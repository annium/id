using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Users;
using Server.ViewModels.Responses.Users;

namespace Server.Host.TestClient.Clients;

public class UserClient
{
    private readonly IHttpRequest _request;

    internal UserClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IHttpResponse<IResult<UserResponse[]>>> FindUsersAsync(
        string query,
        int limit,
        IResult<UserResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Get("users")
            .Param("query", query)
            .Param("limit", limit)
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<UserResponse>>> GetUserAsync(
        Guid userId,
        IResult<UserResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get($"users/{userId}").AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> AddRoleToUserAsync(
        Guid userId,
        Guid roleId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post($"users/{userId}/roles/{roleId}").AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> DeleteRoleFromUserAsync(
        Guid userId,
        Guid roleId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Delete($"users/{userId}/roles/{roleId}").AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> AddClaimToUserAsync(
        Guid userId,
        Guid claimId,
        AddClaimToUserRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post($"users/{userId}/claims/{claimId}")
            .JsonContent(body)
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> DeleteClaimFromUserAsync(
        Guid userId,
        Guid claimId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Delete($"users/{userId}/claims/{claimId}").AsResponseAsync(defaultValue, ct);
    }
}
