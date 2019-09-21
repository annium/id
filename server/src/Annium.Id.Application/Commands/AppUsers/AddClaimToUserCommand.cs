using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.AppUsers
{
    public class AddClaimToUserCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid UserId { get; }
        public Guid ClaimId { get; }
        public string Value { get; }
        public Guid MyId { get; private set; }
        public App App { get; private set; }
        public User User { get; private set; }
        public Claim Claim { get; private set; }

        public AddClaimToUserCommand(
            Guid appId,
            Guid userId,
            Guid claimId,
            string value
        )
        {
            AppId = appId;
            UserId = userId;
            ClaimId = claimId;
            Value = value;
        }
    }

    internal class AddClaimToUserCommandValidator : Validator<AddClaimToUserCommand>
    {
        public AddClaimToUserCommandValidator()
        {
            Field(c => c.AppId).Required();
            Field(c => c.UserId).Required();
            Field(c => c.ClaimId).Required();
            Field(c => c.Value).Required().Length(3, 100);
        }
    }

    internal class AddClaimToUserCommandComposer : Composer<AddClaimToUserCommand>
    {
        public AddClaimToUserCommandComposer(
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