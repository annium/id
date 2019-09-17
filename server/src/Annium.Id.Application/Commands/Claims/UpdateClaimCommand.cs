using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Claims
{
    public class UpdateClaimCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid ClaimId { get; }
        public string Key { get; }
        public string Name { get; }
        public Guid UserId { get; private set; }
        public App App { get; private set; }
        public Claim Claim { get; private set; }

        public UpdateClaimCommand(
            Guid appId,
            Guid claimId,
            string key,
            string name
        )
        {
            AppId = appId;
            ClaimId = claimId;
            Key = key;
            Name = name;
        }
    }

    internal class UpdateClaimCommandValidator : Validator<UpdateClaimCommand>
    {
        public UpdateClaimCommandValidator()
        {
            Field(c => c.AppId).NotEqual(Guid.Empty);
            Field(c => c.ClaimId).NotEqual(Guid.Empty);
            Field(c => c.Key).Required().Length(3, 100);
            Field(c => c.Name).Required().Length(3, 100);
        }
    }

    internal class UpdateClaimCommandComposer : Composer<UpdateClaimCommand>
    {
        public UpdateClaimCommandComposer(
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