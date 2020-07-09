using System;
using System.Threading.Tasks;
using Annium.Id.Api.ViewModels.Requests.Companies;
using Annium.Id.Api.ViewModels.Responses.Companies;

namespace Annium.Id.Api.TestClient.Clients
{
    public static class CompanyClientExtensions
    {
        public static async Task<CompanyResponse> Register(
            this CompanyClient client,
            Guid? parentId = default,
            string name = "First Company"
        )
        {
            var request = new RegisterCompanyRequest { ParentId = parentId, Name = name };
            var companyId = await client.RegisterCompany(request).GetData();
            var company = await client.GetCompanyInfo(companyId).GetData();

            return company;
        }

        public static Task<CompanyResponse> RegisterOther(
            this CompanyClient client,
            Guid? parentId = default,
            string name = "Second Company"
        ) => client.Register(parentId, name);
    }
}