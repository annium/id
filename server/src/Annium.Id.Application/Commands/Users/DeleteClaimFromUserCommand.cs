using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Users
{
    public class DeleteClaimFromUserCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid ClaimId { get; }
        public Guid MyId { get; private set; }
        public User User { get; private set; }
        public Claim Claim { get; private set; }

        public DeleteClaimFromUserCommand(
            Guid userId,
            Guid claimId
        )
        {
            UserId = userId;
            ClaimId = claimId;
        }
    }

    internal class DeleteClaimFromUserCommandValidator : Validator<DeleteClaimFromUserCommand>
    {
        public DeleteClaimFromUserCommandValidator()
        {
            Field(c => c.UserId).Required();
            Field(c => c.ClaimId).Required();
        }
    }

    internal class DeleteClaimFromUserCommandComposer : Composer<DeleteClaimFromUserCommand>
    {
        public DeleteClaimFromUserCommandComposer(
            ITokenAccessor tokenAccessor,
            IUserRepository userRepository,
            IClaimRepository claimRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.User).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.UserId));
            Field(c => c.Claim).LoadWith(ctx => claimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}