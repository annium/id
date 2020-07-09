using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Tools
{
    internal class TokenGenerator : ITokenGenerator
    {
        private readonly IAppRepository appRepository;
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IUserClaimRepository userClaimRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly ICompanyUserRoleRepository companyUserRoleRepository;
        private readonly ICompanyUserClaimRepository companyUserClaimRepository;
        private readonly ITokenWriter tokenWriter;

        public TokenGenerator(
            IAppRepository appRepository,
            IUserRoleRepository userRoleRepository,
            IUserClaimRepository userClaimRepository,
            ICompanyRepository companyRepository,
            ICompanyUserRoleRepository companyUserRoleRepository,
            ICompanyUserClaimRepository companyUserClaimRepository,
            ITokenWriter tokenWriter
        )
        {
            this.appRepository = appRepository;
            this.userRoleRepository = userRoleRepository;
            this.userClaimRepository = userClaimRepository;
            this.companyRepository = companyRepository;
            this.companyUserRoleRepository = companyUserRoleRepository;
            this.companyUserClaimRepository = companyUserClaimRepository;
            this.tokenWriter = tokenWriter;
        }

        public async Task<string> GenerateToken(UserLogin login)
        {
            var app = await appRepository.GetByIdAsync(login.AppId);

            var userRoles = await userRoleRepository.GetUserRolesAsync(app.Id, login.UserId);
            var userClaims = await userClaimRepository.GetUserClaimsAsync(app.Id, login.UserId);

            var appToken = BuildAppToken(app, userRoles, userClaims);

            var companyUserRoles = await companyUserRoleRepository.GetCompaniesUserRolesAsync(app.Id, login.UserId);
            var companyUserClaims = await companyUserClaimRepository.GetCompaniesUserClaimsAsync(app.Id, login.UserId);

            var companyIds = companyUserRoles.Keys.Union(companyUserClaims.Keys).ToArray();
            var companies = await companyRepository.GetAllByIdsAsync(companyIds);
            var companyTokens = companies.Select(company => BuildCompanyToken(
                company,
                companyUserRoles.ContainsKey(company.Id) ? companyUserRoles[company.Id] : Array.Empty<CompanyRole>(),
                companyUserClaims.ContainsKey(company.Id) ? companyUserClaims[company.Id] : Array.Empty<ClaimValue>()
            )).ToArray();

            var token = new IdToken(login.UserId, login.Id, appToken, companyTokens);

            return tokenWriter.WriteToken(token);
        }

        private AppToken BuildAppToken(
            App app,
            Role[] userRoles,
            ClaimValue[] userClaims
        )
        {
            var roles = userRoles.Select(r => r.Key).ToArray();

            var claims = new Dictionary<string, string>();
            foreach (var role in userRoles.OrderBy(ur => ur.Key))
            foreach (var roleClaim in role.Claims)
                claims[roleClaim.Key] = roleClaim.Value;
            foreach (var userClaim in userClaims)
                claims[userClaim.Key] = userClaim.Value;

            return new AppToken(app.Id, roles, claims);
        }

        private CompanyToken BuildCompanyToken(
            Company company,
            CompanyRole[] userRoles,
            ClaimValue[] userClaims
        )
        {
            var roles = userRoles.Select(r => r.Key).ToArray();

            var claims = new Dictionary<string, string>();
            foreach (var role in userRoles.OrderBy(ur => ur.Key))
            foreach (var roleClaim in role.Claims)
                claims[roleClaim.Key] = roleClaim.Value;
            foreach (var userClaim in userClaims)
                claims[userClaim.Key] = userClaim.Value;

            return new CompanyToken(company.Id, roles, claims);
        }
    }
}