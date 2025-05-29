using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Server.ViewModels.Requests.Companies;
using Server.ViewModels.Responses.Companies;
using static Server.Host.TestClient.Clients.Helper;

namespace Server.Host.TestClient.Clients;

public static class CompanyClientExtensions
{
    public static async Task<CompanyResponse> RegisterAsync(
        this CompanyClient client,
        Guid? parentId = default,
        string? name = null
    )
    {
        var request = new RegisterCompanyRequest { ParentId = parentId, Name = name ?? Faker.Random.String2(10) };
        var companyId = await client
            .RegisterCompanyAsync(request, Result.New(Guid.Empty).Error("Failed to register company"))
            .GetDataAsync();
        var company = await client
            .GetCompanyAsync(companyId, Result.New(new CompanyResponse()).Error("Failed to load company information"))
            .GetDataAsync();

        return company;
    }
}
