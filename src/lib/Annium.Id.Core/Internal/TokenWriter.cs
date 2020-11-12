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
        private readonly RsaSecurityKey _signingKey;
        private readonly AuthOptions _options;
        private readonly Func<Instant> _getInstant;

        public TokenWriter(
            AuthOptions options,
            Func<Instant> getInstant
        )
        {
            using (var s = File.OpenRead(options.PrivateKeyFile))
            {
                var provider = new RSACryptoServiceProvider();
                provider.ImportParameters(new KeyReader().ReadRsaKey(s));
                _signingKey = new RsaSecurityKey(provider);
            }

            _options = options;
            _getInstant = getInstant;
        }

        public string WriteToken(IdToken token)
        {
            var packedToken = Convert.ToBase64String(MessagePackSerializer.Serialize(
                token,
                MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray)
            ));

            var instant = _getInstant();
            var now = instant.ToDateTimeUtc();
            var expires = (instant + _options.AccessTokenLifeTime).ToDateTimeUtc();

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
                signingCredentials: new SigningCredentials(_signingKey, SecurityAlgorithms.RsaSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}