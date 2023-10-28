using System;
using System.Threading;
using System.Threading.Tasks;
using Annium;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Annium.Id.Core;
using Server.Application.Services;
using Server.Application.Tools;
using Server.Db.Repositories;
using Server.Domain.Commands.Login;
using Server.Domain.Models;

namespace Server.Application.CommandHandlers;

internal class LoginCommandHandler
    : ICommandHandler<LogInCommand, Tokens>,
        ICommandHandler<LogOutCommand>,
        ICommandHandler<UpdateTokensCommand, Tokens>
{
    private readonly ITimeProvider _timeProvider;
    private readonly AuthOptions _options;
    private readonly IUserLoginRepository _userLoginRepository;
    private readonly ISecurityManager _securityManager;
    private readonly ILoginService _loginService;
    private readonly ITokenGenerator _tokenGenerator;

    public LoginCommandHandler(
        ITimeProvider timeProvider,
        AuthOptions options,
        IUserLoginRepository userLoginRepository,
        ISecurityManager securityManager,
        ILoginService loginService,
        ITokenGenerator tokenGenerator
    )
    {
        _timeProvider = timeProvider;
        _options = options;
        _userLoginRepository = userLoginRepository;
        _securityManager = securityManager;
        _loginService = loginService;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<IStatusResult<OperationStatus, Tokens>> HandleAsync(
        LogInCommand request,
        CancellationToken cancellationToken
    )
    {
        var app = request.App;
        var user = request.User;

        if (_securityManager.Hash(request.Password) != user.PasswordHash)
            return Result.Status(OperationStatus.Forbidden, default(Tokens)!).Error("Invalid password");

        var tokens = await _loginService.LogUserInAsync(app, user);

        return Result.Status(OperationStatus.Ok, tokens);
    }

    public async Task<IStatusResult<OperationStatus, Tokens>> HandleAsync(
        UpdateTokensCommand request,
        CancellationToken cancellationToken
    )
    {
        var login = request.Login;

        if (login.RefreshTokenExpires < _timeProvider.Now)
            return Result.Status(OperationStatus.Forbidden, default(Tokens)!).Error("Refresh token expired");

        login.Update(Guid.NewGuid(), _timeProvider.Now + _options.RefreshTokenLifeTime);
        await _userLoginRepository.UpdateRefreshTokenAsync(login);
        var token = await _tokenGenerator.GenerateTokenString(login);

        return Result.Status(OperationStatus.Ok, new Tokens(token, login.RefreshToken, login.RefreshTokenExpires));
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        LogOutCommand request,
        CancellationToken cancellationToken
    )
    {
        await _userLoginRepository.DeleteByIdAsync(request.LoginId);

        return Result.Status(OperationStatus.Ok);
    }
}
