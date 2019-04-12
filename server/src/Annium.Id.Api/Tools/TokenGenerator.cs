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
    internal class TokenGenerator : ITokenGenerator, IDisposable
    {
        private readonly TimeSpan tokenLifeTime = TimeSpan.FromDays(1);

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
                login.IPAddress,
                login.Client
            );

            var claims = new [] { new SystemClaim("id", Convert.ToBase64String(LZ4MessagePackSerializer.Serialize(token))) };

            var jwt = new JwtSecurityToken(
                issuer: "annium.id",
                audience: "api",
                claims : claims,
                expires : DateTime.Now + tokenLifeTime,
                signingCredentials : new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}