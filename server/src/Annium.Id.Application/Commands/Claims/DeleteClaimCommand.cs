using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Claims
{
    public class DeleteClaimCommand : ICommand
    {
        public Guid ClaimId { get; }
        public Guid MyId { get; private set; }
        public Claim Claim { get; private set; } = null!;

        public DeleteClaimCommand(
            Guid claimId
        )
        {
            ClaimId = claimId;
        }
    }

    internal class DeleteClaimCommandValidator : Validator<DeleteClaimCommand>
    {
        public DeleteClaimCommandValidator()
        {
            Field(c => c.ClaimId).Required();
        }
    }

    internal class DeleteClaimCommandComposer : Composer<DeleteClaimCommand>
    {
        public DeleteClaimCommandComposer(
            ITokenAccessor tokenAccessor,
            IClaimRepository claimRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.Claim).LoadWith(ctx => claimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}