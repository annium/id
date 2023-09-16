using System;
using System.IO;
using Annium.Identity.Tokens;
using Annium.Identity.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using SystemClaim = System.Security.Claims.Claim;

namespace Annium.Id.Core.Internal;

internal class TokenWriter : ITokenWriter
{
    private readonly RsaSecurityKey _securityKey;
    private readonly AuthOptions _options;
    private readonly ITimeProvider _timeProvider;

    public TokenWriter(
        AuthOptions options,
        ITimeProvider timeProvider
    )
    {
        _securityKey = KeyReader.ReadRsaKey(File.ReadAllText(options.PrivateKeyFile));
        _options = options;
        _timeProvider = timeProvider;
    }

    public string WriteToken(IdToken token)
    {
        var instant = _timeProvider.Now;
        var packedToken = Serializer.Serialize(token);

        var jwt = JwtWriter.Create(
            _securityKey,
            Guid.NewGuid().ToString(),
            Constants.Issuer,
            token.App.Id.ToString(),
            instant,
            _options.AccessTokenLifeTime,
            (Claims.Id, packedToken)
        );

        var raw = jwt.GetString();

        return raw;
    }
}