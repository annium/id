using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Annium.Extensions.Primitives;
using Annium.Id.Application.Commands.Me;
using Annium.Id.Application.Services;
using Annium.Id.Application.Tools;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Email;

namespace Annium.Id.Application.CommandHandlers
{
    internal class MeCommandHandler :
        ICommandHandler<RegisterMeCommand>,
        ICommandHandler<ConfirmMyEmailCommand, Tokens>,
        ICommandHandler<RestoreMyAccessCommand>,
        ICommandHandler<UpdateMeCommand>,
        ICommandHandler<UnregisterMeCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly IUserLoginRepository userLoginRepository;
        private readonly ISecurityManager securityManager;
        private readonly ILoginService loginService;
        private readonly IEmailService emailService;

        public MeCommandHandler(
            IUserRepository userRepository,
            IUserLoginRepository userLoginRepository,
            ISecurityManager securityManager,
            ILoginService loginService,
            IEmailService emailService
        )
        {
            this.userRepository = userRepository;
            this.userLoginRepository = userLoginRepository;
            this.securityManager = securityManager;
            this.loginService = loginService;
            this.emailService = emailService;
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            RegisterMeCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = new User(
                request.Login,
                string.Empty,
                request.Email
            );

            user = await userRepository.CreateAsync(user);

            var result = await emailService.SendEmailConfirmationAsync(user);
            if (result.IsFailure)
                return Result.Status(OperationStatus.UncaughtException).Join(result);

            return Result.Status(OperationStatus.OK);
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
            user.PasswordHash = securityManager.Hash(Guid.NewGuid().ToString());
            await userRepository.UpdateAsync(user);

            var tokens = await loginService.LogUserInAsync(app, user);

            return Result.Status(OperationStatus.OK, tokens);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            RestoreMyAccessCommand request,
            CancellationToken cancellationToken
        )
        {
            var app = request.App;
            var user = request.User;

            var tokens = await loginService.LogUserInAsync(app, user);

            var result = await emailService.SendRestoreAccessAsync(user, tokens);
            if (result.IsFailure)
                return Result.Status(OperationStatus.UncaughtException).Join(result);

            return Result.Status(OperationStatus.OK);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            UpdateMeCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = request.User;

            if (request.Login != user.Login && (await userRepository.FindByLoginAsync(request.Login)) != null)
                return Result.Status(OperationStatus.Conflict).Error($"Login {request.Login} is already used");

            if (request.Email != user.Email && (await userRepository.FindByEmailAsync(request.Email)) != null)
                return Result.Status(OperationStatus.Conflict).Error($"Email {request.Email} is already used");

            user.Login = request.Login;
            user.PasswordHash = securityManager.Hash(request.Password);
            user.Email = request.Email;

            await userRepository.UpdateAsync(user);

            return Result.Status(OperationStatus.OK);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            UnregisterMeCommand request,
            CancellationToken cancellationToken
        )
        {
            await userLoginRepository.DeleteAllByUserIdAsync(request.MyId);
            await userRepository.DeleteByIdAsync(request.MyId);

            return Result.Status(OperationStatus.OK);
        }
    }
}