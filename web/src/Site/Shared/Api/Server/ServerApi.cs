using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Annium;
using Annium.Blazor.Net;
using Annium.Core.DependencyInjection;
using Annium.Data.Operations;
using Annium.Net.Http;
using Annium.Serialization.Abstractions;
using Server.ViewModels.Responses.Login;
using Site.Shared.Api.Server.Clients;
using Site.Shared.Stores;

namespace Site.Shared.Api.Server;

internal class ServerApi : IServerApi
{
    public IHttpRequest Public => _requestFactory.New(_config.Server);

    public IHttpRequest Private =>
        _requestFactory
            .New(_config.Server)
            .BearerAuthorization(_tokenStore.Get()?.AccessToken ?? string.Empty)
            .Intercept(AuthMiddlewareAsync);

    public IHttpRequest PrivateBase =>
        _requestFactory.New(_config.Server).BearerAuthorization(_tokenStore.Get()?.AccessToken ?? string.Empty);

    private readonly ITimeProvider _timeProvider;
    private readonly IHttpRequestFactory _requestFactory;
    private readonly ITokenStore _tokenStore;
    private readonly Configuration _config;
    private readonly ISerializer<string> _serializer;

    public ServerApi(
        IServiceProvider sp,
        ITimeProvider timeProvider,
        IHttpRequestFactory requestFactory,
        ITokenStore tokenStore,
        Configuration config
    )
    {
        _timeProvider = timeProvider;
        var serializerKey = SerializerKey.CreateDefault(MediaTypeNames.Application.Json);
        _serializer = sp.ResolveKeyed<ISerializer<string>>(serializerKey);
        _requestFactory = requestFactory;
        _tokenStore = tokenStore;
        _config = config;
    }

    private async Task<IHttpResponse> AuthMiddlewareAsync(Func<Task<IHttpResponse>> next)
    {
        var response = await next();

        // if response succeed or any failure except Unauthorized - return response as is
        if (response.IsSuccess || response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        var tokens = _tokenStore.Get();
        if (tokens is null)
            return GetAuthFailureResponse(response.Uri, "No user token available to perform token update");

        // if unauthorized - try refresh token and retry
        if (tokens.RefreshTokenExpires < _timeProvider.Now)
            return GetAuthFailureResponse(response.Uri, "Refresh token is expired. Need to login");

        var updateTokenResult = await Private
            .Client()
            .Login.UpdateTokenAsync(
                _config.AppId,
                tokens.RefreshToken,
                Result.Create(new TokensResponse()).Error("Failed to update token")
            );
        if (updateTokenResult.HasErrors)
            return response;

        return await next();
    }

    /// <summary>
    /// Builds the 401 handed back when the middleware gives up before reaching the server.
    /// </summary>
    /// <remarks>
    /// This response is synthesized locally, so it carries the URI of the call that failed rather than
    /// one of its own - <see cref="HttpResponse.Result"/> is the factory for a real (non-network,
    /// non-aborted) unsuccessful response, which is what a refused auth retry is.
    /// </remarks>
    /// <param name="uri">The URI of the request that came back unauthorized.</param>
    /// <param name="failure">Why the token could not be refreshed.</param>
    /// <returns>An unsuccessful response carrying the failure as an operation result.</returns>
    private IHttpResponse GetAuthFailureResponse(Uri uri, string failure)
    {
        var content = new StringContent(_serializer.Serialize(Result.Create().Error(failure)), Encoding.UTF8);

        return HttpResponse.Result(
            isSuccess: false,
            uri,
            HttpStatusCode.Unauthorized,
            nameof(HttpStatusCode.Unauthorized),
            HttpResponse.EmptyHeaders,
            content
        );
    }
}

public interface IServerApi : IApi
{
    IHttpRequest Public { get; }
    IHttpRequest Private { get; }
    IHttpRequest PrivateBase { get; }
}
