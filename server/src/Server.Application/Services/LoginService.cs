using System;
using System.Threading.Tasks;
using Annium;
using Annium.Id.Core;
using Server.Application.Tools;
using Server.Db.Repositories;
using Server.Domain.Models;

namespace Server.Application.Services;

internal class LoginService : ILoginService
{
    private readonly ITimeProvider _timeProvider;
    private readonly AuthOptions _options;
    private readonly IUserLoginRepository _userLoginRepository;
    private readonly IIdentityDataAccessor _identityDataAccessor;
    private readonly ITokenGenerator _tokenGenerator;

    public LoginService(
        ITimeProvider timeProvider,
        AuthOptions options,
        IUserLoginRepository userLoginRepository,
        IIdentityDataAccessor identityDataAccessor,
        ITokenGenerator tokenGenerator
    )
    {
        _timeProvider = timeProvider;
        _options = options;
        _userLoginRepository = userLoginRepository;
        _identityDataAccessor = identityDataAccessor;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Tokens> LogUserInAsync(App app, User user)
    {
        var instant = _timeProvider.Now;
        var identity = _identityDataAccessor.GetIdentityData();
        var login = new UserLogin(
            app,
            user,
            instant,
            identity.IpAddress.ToString(),
            identity.Client,
            Guid.NewGuid(),
            instant + _options.RefreshTokenLifeTime
        );

        await _userLoginRepository.DeleteExpiredByUserIdAsync(user.Id, instant);
        await _userLoginRepository.CreateAsync(login);

        var token = await _tokenGenerator.GenerateTokenString(login);

        return new Tokens(token, login.RefreshToken, login.RefreshTokenExpires);
    }
}