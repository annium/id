using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Users
{
    public class AddClaimToUserCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid ClaimId { get; }
        public string Value { get; }
        public Guid MyId { get; private set; }
        public User User { get; private set; } = null!;
        public Claim Claim { get; private set; } = null!;

        public AddClaimToUserCommand(
            Guid userId,
            Guid claimId,
            string value
        )
        {
            UserId = userId;
            ClaimId = claimId;
            Value = value;
        }
    }

    internal class AddClaimToUserCommandValidator : Validator<AddClaimToUserCommand>
    {
        public AddClaimToUserCommandValidator()
        {
            Field(c => c.UserId).Required();
            Field(c => c.ClaimId).Required();
            Field(c => c.Value).Required().Length(3, 100);
        }
    }

    internal class AddClaimToUserCommandComposer : Composer<AddClaimToUserCommand>
    {
        public AddClaimToUserCommandComposer(
            ITokenAccessor tokenAccessor,
            IUserRepository userRepository,
            IClaimRepository claimRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.User).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.UserId));
            Field(c => c.Claim).LoadWith(ctx => claimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}