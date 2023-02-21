using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Users;
using Server.ViewModels.Responses.Users;

namespace Site.Shared.Api.Server.Clients;

public class UserClient : ClientBase
{
    public UserClient(IHttpRequest request) : base(request)
    {
    }

    public async Task<IResult<UserResponse[]>> FindUsers(
        string query,
        int limit
    )
    {
        return await Request.Clone()
            .Get("users")
            .Param("query", query)
            .Param("limit", limit)
            .AsAsync<IResult<UserResponse[]>>();
    }

    public async Task<IResult<UserResponse>> GetUser(
        Guid userId
    )
    {
        return await Request.Clone()
            .Get($"users/{userId}")
            .AsAsync<IResult<UserResponse>>();
    }

    public async Task<IResult> AddRoleToUser(
        Guid userId,
        Guid roleId
    )
    {
        return await Request.Clone()
            .Post($"users/{userId}/roles/{roleId}")
            .AsAsync<IResult>();
    }

    public async Task<IResult> DeleteRoleFromUser(
        Guid userId,
        Guid roleId
    )
    {
        return await Request.Clone()
            .Delete($"users/{userId}/roles/{roleId}")
            .AsAsync<IResult>();
    }

    public async Task<IResult> AddClaimToUser(
        Guid userId,
        Guid claimId,
        AddClaimToUserRequestBody body
    )
    {
        return await Request.Clone()
            .Post($"users/{userId}/claims/{claimId}")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult> DeleteClaimFromUser(
        Guid userId,
        Guid claimId
    )
    {
        return await Request.Clone()
            .Delete($"users/{userId}/claims/{claimId}")
            .AsAsync<IResult>();
    }
}