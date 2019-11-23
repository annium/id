using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Annium.Id.Application.Commands.Me;
using Annium.Id.Application.Tools;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.CommandHandlers
{
    internal class MeCommandHandler :
        ICommandHandler<RegisterMeCommand, Guid>,
        ICommandHandler<UpdateMeCommand>,
        ICommandHandler<UnregisterMeCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly IUserLoginRepository userLoginRepository;
        private readonly ISecurityManager securityManager;

        public MeCommandHandler(
            IUserRepository userRepository,
            IUserLoginRepository userLoginRepository,
            ISecurityManager securityManager
        )
        {
            this.userRepository = userRepository;
            this.userLoginRepository = userLoginRepository;
            this.securityManager = securityManager;
        }

        public async Task<IStatusResult<OperationStatus, Guid>> HandleAsync(
            RegisterMeCommand request,
            CancellationToken cancellationToken
        )
        {
            var passwordHash = securityManager.Hash(request.Password);

            var user = new User(
                request.Login,
                passwordHash,
                request.Email
            );

            user = await userRepository.CreateAsync(user);

            return Result.Status(OperationStatus.OK, user.Id);
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