using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.Roles;
using Annium.Id.Api.ViewModels.Responses.Roles;
using Annium.Net.Http;

namespace Annium.Id.Api.TestClient.Clients
{
    public static class RoleClientExtensions
    {
        public static async Task<RoleResponse> Register(
            this RoleClient client,
            Guid appId,
            string key = "one",
            string name = "First role"
        )
        {
            var request = new CreateRoleRequest { AppId = appId, Key = key, Name = name };
            var roleId = await client.CreateRole(request).GetData();
            var roles = await client.ListRoles(appId).GetData();

            return roles.Single(x => x.Id == roleId);
        }

        public static Task<RoleResponse> RegisterOther(
            this RoleClient client,
            Guid appId,
            string key = "two",
            string name = "Second role"
        ) => client.Register(appId, key, name);

        public static Task<IHttpResponse<IResult>> AddClaimToRole(
            this RoleClient client,
            Guid claimId,
            Guid roleId,
            string value = "Some"
        ) => client.AddClaimToRole(claimId, roleId, new AddClaimToRoleRequestBody { Value = value });
    }
}