using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Users;
using Server.ViewModels.Responses.Users;

namespace Site.Shared.Api.Server.Clients;

public class UserClient
{
    private readonly IHttpRequest _request;

    internal UserClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IResult<UserResponse[]>> FindUsers(
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
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult<UserResponse>> GetUser(
        Guid userId,
        IResult<UserResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Get($"users/{userId}")
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult> AddRoleToUser(
        Guid userId,
        Guid roleId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post($"users/{userId}/roles/{roleId}")
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult> DeleteRoleFromUser(
        Guid userId,
        Guid roleId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Delete($"users/{userId}/roles/{roleId}")
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult> AddClaimToUser(
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
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult> DeleteClaimFromUser(
        Guid userId,
        Guid claimId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Delete($"users/{userId}/claims/{claimId}")
            .AsAsync(defaultValue, ct);
    }
}