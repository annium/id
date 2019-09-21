using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.AppUsers
{
    public class DeleteClaimFromUserCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid UserId { get; }
        public Guid ClaimId { get; }
        public Guid MyId { get; private set; }
        public App App { get; private set; }
        public User User { get; private set; }
        public Claim Claim { get; private set; }

        public DeleteClaimFromUserCommand(
            Guid appId,
            Guid userId,
            Guid claimId
        )
        {
            AppId = appId;
            UserId = userId;
            ClaimId = claimId;
        }
    }

    internal class DeleteClaimFromUserCommandValidator : Validator<DeleteClaimFromUserCommand>
    {
        public DeleteClaimFromUserCommandValidator()
        {
            Field(c => c.AppId).Required();
            Field(c => c.UserId).Required();
            Field(c => c.ClaimId).Required();
        }
    }

    internal class DeleteClaimFromUserCommandComposer : Composer<DeleteClaimFromUserCommand>
    {
        public DeleteClaimFromUserCommandComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository,
            IUserRepository userRepository,
            IClaimRepository claimRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(c => c.User).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.UserId));
            Field(c => c.Claim).LoadWith(ctx => claimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}