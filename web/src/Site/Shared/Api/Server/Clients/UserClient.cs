using System;
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
        int limit
    )
    {
        return await _request
            .Get("users")
            .Param("query", query)
            .Param("limit", limit)
            .AsAsync<IResult<UserResponse[]>>();
    }

    public async Task<IResult<UserResponse>> GetUser(
        Guid userId
    )
    {
        return await _request
            .Get($"users/{userId}")
            .AsAsync<IResult<UserResponse>>();
    }

    public async Task<IResult> AddRoleToUser(
        Guid userId,
        Guid roleId
    )
    {
        return await _request
            .Post($"users/{userId}/roles/{roleId}")
            .AsAsync<IResult>();
    }

    public async Task<IResult> DeleteRoleFromUser(
        Guid userId,
        Guid roleId
    )
    {
        return await _request
            .Delete($"users/{userId}/roles/{roleId}")
            .AsAsync<IResult>();
    }

    public async Task<IResult> AddClaimToUser(
        Guid userId,
        Guid claimId,
        AddClaimToUserRequestBody body
    )
    {
        return await _request
            .Post($"users/{userId}/claims/{claimId}")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult> DeleteClaimFromUser(
        Guid userId,
        Guid claimId
    )
    {
        return await _request
            .Delete($"users/{userId}/claims/{claimId}")
            .AsAsync<IResult>();
    }
}