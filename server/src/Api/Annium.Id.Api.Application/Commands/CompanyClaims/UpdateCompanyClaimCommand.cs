using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Application.Commands.CompanyClaims
{
    public class UpdateCompanyClaimCommand : ICommand
    {
        public Guid ClaimId { get; }
        public string Key { get; }
        public string Name { get; }
        public Guid MyId { get; private set; }
        public CompanyClaim Claim { get; private set; } = null!;

        public UpdateCompanyClaimCommand(
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

    internal class UpdateCompanyClaimCommandValidator : Validator<UpdateCompanyClaimCommand>
    {
        public UpdateCompanyClaimCommandValidator()
        {
            Field(c => c.ClaimId).Required();
            Field(c => c.Key).Required().Length(3, 100);
            Field(c => c.Name).Required().Length(3, 100);
        }
    }

    internal class UpdateCompanyClaimCommandComposer : Composer<UpdateCompanyClaimCommand>
    {
        public UpdateCompanyClaimCommandComposer(
            ITokenAccessor tokenAccessor,
            ICompanyClaimRepository companyClaimRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.Claim).LoadWith(ctx => companyClaimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}