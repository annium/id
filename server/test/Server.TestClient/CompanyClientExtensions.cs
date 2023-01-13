using System;
using System.Threading.Tasks;
using Server.ViewModels.Requests.Companies;
using Server.ViewModels.Responses.Companies;

namespace Server.TestClient;

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
        var company = await client.GetCompany(companyId).GetData();

        return company;
    }

    public static Task<CompanyResponse> RegisterOther(
        this CompanyClient client,
        Guid? parentId = default,
        string name = "Second Company"
    )
    {
        return client.Register(parentId, name);
    }
}