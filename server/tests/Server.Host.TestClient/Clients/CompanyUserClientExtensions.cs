using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.CompanyUsers;
using static Server.Host.TestClient.Clients.Helper;

namespace Server.Host.TestClient.Clients;

public static class CompanyUserClientExtensions
{
    public static async Task<IHttpResponse<IResult>> AddUser(
        this CompanyUserClient client,
        Guid companyId,
        Guid userId
    )
    {
        var response = await client.AddUserToCompany(companyId, userId);

        return response;
    }

    public static async Task<IHttpResponse<IResult>> AddUserRole(
        this CompanyUserClient client,
        Guid companyId,
        Guid userId,
        Guid roleId
    )
    {
        var response = await client.AddCompanyRoleToCompanyUser(companyId, userId, roleId);

        return response;
    }

    public static async Task<IHttpResponse<IResult>> AddUserClaim(
        this CompanyUserClient client,
        Guid companyId,
        Guid userId,
        Guid claimId,
        string? value = null
    )
    {
        var response = await client.AddCompanyClaimToCompanyUser(companyId, userId, claimId, new AddCompanyClaimToCompanyUserRequestBody { Value = value ?? Faker.Random.String2(10) });

        return response;
    }
}