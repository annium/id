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
        public Guid AppId { get; }
        public Guid ClaimId { get; }
        public Guid UserId { get; private set; }
        public App App { get; private set; }
        public Claim Claim { get; private set; }

        public DeleteClaimCommand(
            Guid appId,
            Guid claimId
        )
        {
            AppId = appId;
            ClaimId = claimId;
        }
    }

    internal class DeleteClaimCommandValidator : Validator<DeleteClaimCommand>
    {
        public DeleteClaimCommandValidator()
        {
            Field(c => c.AppId).NotEqual(Guid.Empty);
            Field(c => c.ClaimId).NotEqual(Guid.Empty);
        }
    }

    internal class DeleteClaimCommandComposer : Composer<DeleteClaimCommand>
    {
        public DeleteClaimCommandComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository,
            IClaimRepository claimRepository
        )
        {
            Field(c => c.UserId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(c => c.Claim).LoadWith(ctx => claimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}