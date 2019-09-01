using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Annium.Id.Application.Commands;
using Annium.Id.Application.Tools;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.CommandHandlers
{
    public class UserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository userRepository;
        private readonly ISecurityManager securityManager;

        public UserCommandHandler(
            IUserRepository userRepository,
            ISecurityManager securityManager
        )
        {
            this.userRepository = userRepository;
            this.securityManager = securityManager;
        }

        public async Task<IStatusResult<OperationStatus, Guid>> HandleAsync(
            CreateUserCommand request,
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

            return Result.New(OperationStatus.OK, user.Id);
        }
    }
}