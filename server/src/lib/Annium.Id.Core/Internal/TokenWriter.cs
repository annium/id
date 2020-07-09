using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Cryptography;
using Annium.Security.Cryptography;
using MessagePack;
using Microsoft.IdentityModel.Tokens;
using NodaTime;
using SystemClaim = System.Security.Claims.Claim;

namespace Annium.Id.Core.Internal
{
    internal class TokenWriter : ITokenWriter
    {
        private readonly RsaSecurityKey signingKey;
        private readonly AuthOptions options;
        private readonly Func<Instant> getInstant;

        public TokenWriter(
            AuthOptions options,
            Func<Instant> getInstant
        )
        {
            using (var s = File.OpenRead(options.PrivateKeyFile))
            {
                var provider = new RSACryptoServiceProvider();
                provider.ImportParameters(new KeyReader().ReadRsaKey(s));
                signingKey = new RsaSecurityKey(provider);
            }

            this.options = options;
            this.getInstant = getInstant;
        }

        public string WriteToken(IdToken token)
        {
            var packedToken = Convert.ToBase64String(MessagePackSerializer.Serialize(
                token,
                MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray)
            ));

            var instant = getInstant();
            var now = instant.ToDateTimeUtc();
            var expires = (instant + options.AccessTokenLifeTime).ToDateTimeUtc();

            var claims = new List<SystemClaim>
            {
                new SystemClaim(Claims.Id, packedToken),
                new SystemClaim(Claims.IssuedAt, now.ToString()),
                new SystemClaim(Claims.TokenId, Guid.NewGuid().ToString())
            };

            var jwt = new JwtSecurityToken(
                Constants.Issuer,
                token.App.Id.ToString(),
                claims,
                expires: expires,
                notBefore: now,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}