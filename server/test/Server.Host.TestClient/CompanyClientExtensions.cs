using System;
using System.Threading.Tasks;
using Server.ViewModels.Requests.Companies;
using Server.ViewModels.Responses.Companies;
using static Server.Host.TestClient.Helper;

namespace Server.Host.TestClient;

public static class CompanyClientExtensions
{
    public static async Task<CompanyResponse> Register(
        this CompanyClient client,
        Guid? parentId = default,
        string? name = null
    )
    {
        var request = new RegisterCompanyRequest { ParentId = parentId, Name = name ?? Faker.Random.String2(10) };
        var companyId = await client.RegisterCompany(request).GetData();
        var company = await client.GetCompany(companyId).GetData();

        return company;
    }
}