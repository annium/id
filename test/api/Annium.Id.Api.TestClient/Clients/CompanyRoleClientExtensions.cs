using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.CompanyRoles;
using Annium.Id.Api.ViewModels.Responses.CompanyRoles;
using Annium.Net.Http;

namespace Annium.Id.Api.TestClient
{
    public static class CompanyRoleClientExtensions
    {
        public static async Task<CompanyRoleResponse> Register(
            this CompanyRoleClient client,
            Guid appId,
            string key = "one",
            string name = "First role"
        )
        {
            var request = new CreateCompanyRoleRequest { AppId = appId, Key = key, Name = name };
            var roleId = await client.CreateRole(request).GetData();
            var roles = await client.ListRoles(appId).GetData();

            return roles.Single(x => x.Id == roleId);
        }

        public static Task<CompanyRoleResponse> RegisterOther(
            this CompanyRoleClient client,
            Guid appId,
            string key = "two",
            string name = "Second role"
        )
        {
            return client.Register(appId, key, name);
        }

        public static Task<IHttpResponse<IResult>> AddClaimToRole(
            this CompanyRoleClient client,
            Guid claimId,
            Guid roleId,
            string value = "Some"
        )
        {
            return client.AddClaimToRole(claimId, roleId, new AddCompanyClaimToCompanyRoleRequestBody { Value = value });
        }
    }
}