using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Annium.Blazor.Net;
using Annium.Core.DependencyInjection;
using Annium.Core.Primitives;
using Annium.Data.Operations;
using Annium.Id.Site.Shared.Stores;
using Annium.Net.Http;
using Annium.Serialization.Abstractions;

namespace Annium.Id.Site.Shared.Api.Server;

internal class ServerApi : IServerApi
{
    public IHttpRequest Public => _requestFactory.New(_config.Server);

    public IHttpRequest Private => _requestFactory.New(_config.Server)
        .BearerAuthorization(_tokenStore.Get()?.AccessToken ?? string.Empty)
        .Intercept(AuthMiddleware);

    public IHttpRequest PrivateBase => _requestFactory.New(_config.Server)
        .BearerAuthorization(_tokenStore.Get()?.AccessToken ?? string.Empty);

    private readonly ITimeProvider _timeProvider;
    private readonly IHttpRequestFactory _requestFactory;
    private readonly ITokenStore _tokenStore;
    private readonly Configuration _config;
    private readonly ISerializer<string> _serializer;

    public ServerApi(
        ITimeProvider timeProvider,
        IHttpRequestFactory requestFactory,
        ITokenStore tokenStore,
        Configuration config,
        IIndex<string, ISerializer<string>> serializers
    )
    {
        _timeProvider = timeProvider;
        _requestFactory = requestFactory;
        _tokenStore = tokenStore;
        _config = config;
        _serializer = serializers[MediaTypeNames.Application.Json];
    }

    private async Task<IHttpResponse> AuthMiddleware(Func<Task<IHttpResponse>> next)
    {
        var response = await next();

        // if response succeed or any failure except Unauthorized - return response as is
        if (response.IsSuccess || response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        var tokens = _tokenStore.Get();
        if (tokens is null)
            return GetAuthFailureResponse("No user token available to perform token update");

        // if unauthorized - try refresh token and retry
        if (tokens.RefreshTokenExpires < _timeProvider.Now)
            return GetAuthFailureResponse("Refresh token is expired. Need to login");

        var updateTokenResult = await Private.Client().Login.UpdateToken(_config.AppId, tokens.RefreshToken);
        if (updateTokenResult.HasErrors)
            return response;

        return await next();
    }

    private IHttpResponse GetAuthFailureResponse(string failure)
    {
        var message = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        message.Content = new StringContent(_serializer.Serialize(Result.New().Error(failure)), Encoding.UTF8);

        return new HttpResponse(message);
    }
}

public interface IServerApi : IApi
{
    IHttpRequest Public { get; }
    IHttpRequest Private { get; }
    IHttpRequest PrivateBase { get; }
}