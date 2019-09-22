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
        public Guid ClaimId { get; }
        public string Key { get; }
        public string Name { get; }
        public Guid MyId { get; private set; }
        public Claim Claim { get; private set; }

        public UpdateClaimCommand(
            Guid claimId,
            string key,
            string name
        )
        {
            ClaimId = claimId;
            Key = key;
            Name = name;
        }
    }

    internal class UpdateClaimCommandValidator : Validator<UpdateClaimCommand>
    {
        public UpdateClaimCommandValidator()
        {
            Field(c => c.ClaimId).Required();
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
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.Claim).LoadWith(ctx => claimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}