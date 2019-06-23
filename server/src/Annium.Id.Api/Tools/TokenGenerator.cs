using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Cryptography;
using Annium.Id.AspNetCore;
using Annium.Id.Db;
using Annium.Security.Cryptography;
using MessagePack;
using Microsoft.IdentityModel.Tokens;
using SystemClaim = System.Security.Claims.Claim;

namespace Annium.Id.Api.Tools
{
    internal class TokenGenerator : ITokenGenerator
    {
        private readonly TimeSpan tokenLifeTime = TimeSpan.FromMinutes(10);

        private readonly RsaSecurityKey signingKey;

        public TokenGenerator()
        {
            using(var s = File.OpenRead(Path.Combine("keys", "private.key")))
            {
                var provider = new RSACryptoServiceProvider();
                provider.ImportParameters(new KeyReader().ReadRsaKey(s));
                signingKey = new RsaSecurityKey(provider);
            }
        }

        public string Generate(
            UserLogin login
        )
        {
            var token = new IdToken(
                login.UserId,
                login.Id
            );

            var claims = new [] { new SystemClaim(Constants.IdClaim, Convert.ToBase64String(LZ4MessagePackSerializer.Serialize(token))) };

            var jwt = new JwtSecurityToken(
                issuer: Constants.Issuer,
                audience: Constants.Audience,
                claims: claims,
                expires: DateTime.Now + tokenLifeTime,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}