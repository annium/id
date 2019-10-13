using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;
using Annium.Security.Cryptography;
using MessagePack;
using Microsoft.IdentityModel.Tokens;
using NodaTime;
using SystemClaim = System.Security.Claims.Claim;

namespace Annium.Id.Application.Tools
{
    internal class TokenGenerator : ITokenGenerator
    {
        private readonly Duration tokenLifeTime = Duration.FromMinutes(30);
        private readonly RsaSecurityKey signingKey;
        private readonly IAppRepository appRepository;
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IUserClaimRepository userClaimRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly ICompanyUserRoleRepository companyUserRoleRepository;
        private readonly ICompanyUserClaimRepository companyUserClaimRepository;
        private readonly Func<Instant> getInstant;

        public TokenGenerator(
            Configuration configuration,
            IAppRepository appRepository,
            IUserRoleRepository userRoleRepository,
            IUserClaimRepository userClaimRepository,
            ICompanyRepository companyRepository,
            ICompanyUserRoleRepository companyUserRoleRepository,
            ICompanyUserClaimRepository companyUserClaimRepository,
            Func<Instant> getInstant
        )
        {
            using (var s = File.OpenRead(Path.GetFullPath(configuration.PrivateKeyFile)))
            {
                var provider = new RSACryptoServiceProvider();
                provider.ImportParameters(new KeyReader().ReadRsaKey(s));
                signingKey = new RsaSecurityKey(provider);
            }

            this.appRepository = appRepository;
            this.userRoleRepository = userRoleRepository;
            this.userClaimRepository = userClaimRepository;
            this.companyRepository = companyRepository;
            this.companyUserRoleRepository = companyUserRoleRepository;
            this.companyUserClaimRepository = companyUserClaimRepository;
            this.getInstant = getInstant;
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
            ));

            var token = new IdToken(login.UserId, login.Id, appToken, companyTokens);

            return WriteToken(token, app.Key);
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

            return new AppToken(app.Id, app.Key, app.OwnerId, roles, claims);
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

            return new CompanyToken(company.Id, company.Key, company.OwnerId, roles, claims);
        }

        private string WriteToken(object token, string audience)
        {
            var packedToken = Convert.ToBase64String(LZ4MessagePackSerializer.Serialize(token));

            var instant = getInstant();
            var now = instant.ToDateTimeUtc();
            var expires = (instant + tokenLifeTime).ToDateTimeUtc();

            var claims = new List<SystemClaim>
            {
                new SystemClaim(Claims.Id, packedToken),
                new SystemClaim(Claims.IssuedAt, now.ToString()),
                new SystemClaim(Claims.TokenId, Guid.NewGuid().ToString())
            };

            var jwt = new JwtSecurityToken(
                issuer: Constants.Issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                notBefore: now,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}