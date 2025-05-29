using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.CompanyUsers;
using static Server.Host.TestClient.Clients.Helper;

namespace Server.Host.TestClient.Clients;

public static class CompanyUserClientExtensions
{
    public static async Task<IHttpResponse<IResult>> AddUserAsync(
        this CompanyUserClient client,
        Guid companyId,
        Guid userId
    )
    {
        var response = await client.AddUserToCompanyAsync(
            companyId,
            userId,
            Result.New().Error("Failed to add user to company")
        );

        return response;
    }

    public static async Task<IHttpResponse<IResult>> AddUserRoleAsync(
        this CompanyUserClient client,
        Guid companyId,
        Guid userId,
        Guid roleId
    )
    {
        var response = await client.AddCompanyRoleToCompanyUserAsync(
            companyId,
            userId,
            roleId,
            Result.New().Error("Failed to add role to user")
        );

        return response;
    }

    public static async Task<IHttpResponse<IResult>> AddUserClaimAsync(
        this CompanyUserClient client,
        Guid companyId,
        Guid userId,
        Guid claimId,
        string? value = null
    )
    {
        var response = await client.AddCompanyClaimToCompanyUserAsync(
            companyId,
            userId,
            claimId,
            new AddCompanyClaimToCompanyUserRequestBody { Value = value ?? Faker.Random.String2(10) },
            Result.New().Error("Failed to add claim to user")
        );

        return response;
    }
}
