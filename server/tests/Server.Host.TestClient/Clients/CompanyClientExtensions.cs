using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Server.ViewModels.Requests.Companies;
using Server.ViewModels.Responses.Companies;
using static Server.Host.TestClient.Clients.Helper;

namespace Server.Host.TestClient.Clients;

public static class CompanyClientExtensions
{
    public static async Task<CompanyResponse> Register(
        this CompanyClient client,
        Guid? parentId = default,
        string? name = null
    )
    {
        var request = new RegisterCompanyRequest { ParentId = parentId, Name = name ?? Faker.Random.String2(10) };
        var companyId = await client
            .RegisterCompany(request, Result.New(Guid.Empty).Error("Failed to register company"))
            .GetData();
        var company = await client
            .GetCompany(companyId, Result.New(new CompanyResponse()).Error("Failed to load company information"))
            .GetData();

        return company;
    }
}
