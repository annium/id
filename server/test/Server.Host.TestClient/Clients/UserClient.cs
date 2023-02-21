using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Users;
using Server.ViewModels.Responses.Users;

namespace Server.Host.TestClient.Clients;

public class UserClient : ClientBase
{
    public UserClient(IHttpRequest request) : base(request)
    {
    }

    public async Task<IHttpResponse<IResult<UserResponse[]>>> FindUsers(
        string query,
        int limit
    )
    {
        return await Request.Clone()
            .Get("users")
            .Param("query", query)
            .Param("limit", limit)
            .AsResponseAsync<IResult<UserResponse[]>>();
    }

    public async Task<IHttpResponse<IResult<UserResponse>>> GetUser(
        Guid userId
    )
    {
        return await Request.Clone()
            .Get($"users/{userId}")
            .AsResponseAsync<IResult<UserResponse>>();
    }

    public async Task<IHttpResponse<IResult>> AddRoleToUser(
        Guid userId,
        Guid roleId
    )
    {
        return await Request.Clone()
            .Post($"users/{userId}/roles/{roleId}")
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> DeleteRoleFromUser(
        Guid userId,
        Guid roleId
    )
    {
        return await Request.Clone()
            .Delete($"users/{userId}/roles/{roleId}")
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> AddClaimToUser(
        Guid userId,
        Guid claimId,
        AddClaimToUserRequestBody body
    )
    {
        return await Request.Clone()
            .Post($"users/{userId}/claims/{claimId}")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> DeleteClaimFromUser(
        Guid userId,
        Guid claimId
    )
    {
        return await Request.Clone()
            .Delete($"users/{userId}/claims/{claimId}")
            .AsResponseAsync<IResult>();
    }
}