using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.Users;
using Annium.Id.Api.ViewModels.Responses.Users;
using Annium.Net.Http;

namespace Annium.Id.Site.Shared.Api.Server.Clients
{
    public class UserClient : ClientBase
    {
        public UserClient(IHttpRequest request) : base(request)
        {
        }

        public async Task<IResult<IEnumerable<UserResponse>>> FindUsers(
            int limit,
            string query
        )
        {
            return await Request.Clone()
                .Get("users")
                .Param("limit", limit)
                .Param("query", query)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<IEnumerable<UserResponse>>>();
        }

        public async Task<IResult<UserResponse>> GetUser(
            Guid userId
        )
        {
            return await Request.Clone()
                .Get($"users/{userId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<UserResponse>>();
        }

        public async Task<IResult> AddRoleToUser(
            Guid roleId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Post($"users/{userId}/roles/{roleId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> DeleteRoleFromUser(
            Guid roleId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Delete($"users/{userId}/roles/{roleId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> AddClaimToUser(
            Guid claimId,
            Guid userId,
            AddClaimToUserRequestBody body
        )
        {
            return await Request.Clone()
                .Post($"users/{userId}/claims/{claimId}")
                .JsonContent(body)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> DeleteClaimFromUser(
            Guid claimId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Delete($"users/{userId}/claims/{claimId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }
    }
}