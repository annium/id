using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using Annium.Identity.Tokens;
using Annium.Identity.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Annium.Id.Core.Internal;

internal class TokenWriter : ITokenWriter
{
    private readonly JwtWriter _writer;
    private readonly AuthOptions _options;

    public TokenWriter(AuthOptions options, ITimeProvider timeProvider)
    {
        var securityKey = RSA.Create().ImportPem(File.ReadAllText(options.PrivateKeyFile)).GetKey();
        _writer = new JwtWriter(
            new JwtTokensOptions
            {
                SigningKey = securityKey,
                Algorithm = SecurityAlgorithms.RsaSha256,
                Issuer = Constants.Issuer,
                Lifetime = options.AccessTokenLifeTime,
            },
            timeProvider
        );
        _options = options;
    }

    /// <summary>
    /// Packs the token into the id claim and signs it.
    /// </summary>
    /// <remarks>
    /// The audience is the app the token is for, so it varies per token and cannot live in
    /// <see cref="JwtTokensOptions"/> the way the issuer and key do - it goes through
    /// <see cref="JwtWriteOverrides"/> instead. The reader validates against a single configured
    /// audience, which is how a token issued for one app is rejected by another.
    /// </remarks>
    /// <param name="token">The token to write.</param>
    /// <returns>The signed JWT.</returns>
    public string WriteToken(IdToken token)
    {
        var packedToken = Serializer.Serialize(token);
        var identity = new ClaimsIdentity(
            [new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), new Claim(Claims.Id, packedToken)]
        );

        return _writer.Write(
            new ClaimsPrincipal(identity),
            new JwtWriteOverrides(Audience: token.App.Id.ToString(), Lifetime: _options.AccessTokenLifeTime)
        );
    }
}
