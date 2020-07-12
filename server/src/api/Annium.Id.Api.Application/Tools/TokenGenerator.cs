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
        private readonly IAppRepository _appRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserClaimRepository _userClaimRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyUserRoleRepository _companyUserRoleRepository;
        private readonly ICompanyUserClaimRepository _companyUserClaimRepository;
        private readonly ITokenWriter _tokenWriter;

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
            _appRepository = appRepository;
            _userRoleRepository = userRoleRepository;
            _userClaimRepository = userClaimRepository;
            _companyRepository = companyRepository;
            _companyUserRoleRepository = companyUserRoleRepository;
            _companyUserClaimRepository = companyUserClaimRepository;
            _tokenWriter = tokenWriter;
        }

        public async Task<IdToken> GenerateToken(UserLogin login)
        {
            var app = await _appRepository.GetByIdAsync(login.AppId);

            var userRoles = await _userRoleRepository.GetUserRolesAsync(app.Id, login.UserId);
            var userClaims = await _userClaimRepository.GetUserClaimsAsync(app.Id, login.UserId);

            var appToken = BuildAppToken(app, userRoles, userClaims);

            var companyUserRoles = await _companyUserRoleRepository.GetCompaniesUserRolesAsync(app.Id, login.UserId);
            var companyUserClaims = await _companyUserClaimRepository.GetCompaniesUserClaimsAsync(app.Id, login.UserId);

            var companyIds = companyUserRoles.Keys.Union(companyUserClaims.Keys).ToArray();
            var companies = await _companyRepository.GetAllByIdsAsync(companyIds);
            var companyTokens = companies.Select(company => BuildCompanyToken(
                company,
                companyUserRoles.ContainsKey(company.Id) ? companyUserRoles[company.Id] : Array.Empty<CompanyRole>(),
                companyUserClaims.ContainsKey(company.Id) ? companyUserClaims[company.Id] : Array.Empty<ClaimValue>()
            )).ToArray();

            var token = new IdToken(login.UserId, login.Id, appToken, companyTokens);

            return token;
        }

        public async Task<string> GenerateTokenString(UserLogin login)
        {
            var token = await GenerateToken(login);

            return _tokenWriter.WriteToken(token);
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