using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Users.Requests;
using Annium.Net.Http;

namespace Annium.Id.Api.TestClient.Clients
{
    public class UserClient : ClientBase
    {
        public UserClient(IHttpRequest request) : base(request)
        {
        }

        public async Task<IHttpResponse<IResult>> AddRoleToUser(
            Guid roleId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Post($"users/{userId}/roles/{roleId}")
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> DeleteRoleFromUser(
            Guid roleId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Delete($"users/{userId}/roles/{roleId}")
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> AddClaimToUser(
            Guid claimId,
            Guid userId,
            AddClaimToUserRequestBody body
        )
        {
            return await Request.Clone()
                .Post($"users/{userId}/claims/{claimId}")
                .JsonContent(body)
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> DeleteClaimFromUser(
            Guid claimId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Delete($"users/{userId}/claims/{claimId}")
                .AsResponseAsync<IResult>();
        }
    }
}