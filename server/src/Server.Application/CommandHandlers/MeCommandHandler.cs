using System;
using System.Threading;
using System.Threading.Tasks;
using Annium;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Server.Application.Services;
using Server.Application.Tools;
using Server.Db.Repositories;
using Server.Domain.Commands.Me;
using Server.Domain.Models;
using Server.Email;

namespace Server.Application.CommandHandlers;

internal class MeCommandHandler :
    ICommandHandler<RegisterMeCommand>,
    ICommandHandler<ConfirmMyEmailCommand, Tokens>,
    ICommandHandler<RestoreMyAccessCommand>,
    ICommandHandler<UpdateMyPasswordCommand>,
    ICommandHandler<UpdateMyProfileCommand>,
    ICommandHandler<UnregisterMeCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserLoginRepository _userLoginRepository;
    private readonly ISecurityManager _securityManager;
    private readonly ILoginService _loginService;
    private readonly IEmailService _emailService;

    public MeCommandHandler(
        IUserRepository userRepository,
        IUserLoginRepository userLoginRepository,
        ISecurityManager securityManager,
        ILoginService loginService,
        IEmailService emailService
    )
    {
        _userRepository = userRepository;
        _userLoginRepository = userLoginRepository;
        _securityManager = securityManager;
        _loginService = loginService;
        _emailService = emailService;
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        RegisterMeCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = new User(
            request.Login,
            string.Empty,
            request.Email,
            request.Referral
        );

        await _userRepository.CreateAsync(user);

        var result = await _emailService.SendEmailConfirmationAsync(user, request.ServerUri);
        if (result.IsFailure)
            return Result.Status(OperationStatus.UncaughtError).Join(result);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus, Tokens>> HandleAsync(
        ConfirmMyEmailCommand request,
        CancellationToken cancellationToken
    )
    {
        var app = request.App;
        var user = request.User;

        // if password is already set - user has already confirmed email
        if (!user.PasswordHash.IsNullOrWhiteSpace())
            return Result.Status<OperationStatus, Tokens>(OperationStatus.Forbidden, default!).Error("Email already confirmed");

        // set random password to allow check above be bypassed only once
        var passwordHash = _securityManager.Hash(Guid.NewGuid().ToString());
        user.SetPasswordHash(passwordHash);
        await _userRepository.UpdateAsync(user);

        var tokens = await _loginService.LogUserInAsync(app, user);

        return Result.Status(OperationStatus.Ok, tokens);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        RestoreMyAccessCommand request,
        CancellationToken cancellationToken
    )
    {
        var app = request.App;
        var user = request.User;

        var tokens = await _loginService.LogUserInAsync(app, user);

        var result = await _emailService.SendRestoreAccessAsync(user, request.ServerUri, tokens);
        if (result.IsFailure)
            return Result.Status(OperationStatus.UncaughtError).Join(result);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        UpdateMyPasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = request.User;

        var passwordHash = _securityManager.Hash(request.Password);
        user.SetPasswordHash(passwordHash);

        await _userRepository.UpdateAsync(user);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        UpdateMyProfileCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = request.User;

        if (request.Login != user.Login && await _userRepository.TryFindByLoginAsync(request.Login) != null)
            return Result.Status(OperationStatus.Conflict).Error($"Login {request.Login} is already used");

        if (request.Email != user.Email && await _userRepository.TryFindByEmailAsync(request.Email) != null)
            return Result.Status(OperationStatus.Conflict).Error($"Email {request.Email} is already used");

        user.Update(request.Login, request.Email);

        await _userRepository.UpdateAsync(user);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        UnregisterMeCommand request,
        CancellationToken cancellationToken
    )
    {
        await _userLoginRepository.DeleteAllByUserIdAsync(request.MyId);
        await _userRepository.DeleteByIdAsync(request.MyId);

        return Result.Status(OperationStatus.Ok);
    }
}