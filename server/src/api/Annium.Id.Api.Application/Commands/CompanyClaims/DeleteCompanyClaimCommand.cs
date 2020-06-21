using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Application.Commands.CompanyClaims
{
    public class DeleteCompanyClaimCommand : ICommand
    {
        public Guid ClaimId { get; }
        public Guid MyId { get; private set; }
        public CompanyClaim Claim { get; private set; } = null!;

        public DeleteCompanyClaimCommand(
            Guid claimId
        )
        {
            ClaimId = claimId;
        }
    }

    internal class DeleteCompanyClaimCommandValidator : Validator<DeleteCompanyClaimCommand>
    {
        public DeleteCompanyClaimCommandValidator()
        {
            Field(c => c.ClaimId).Required();
        }
    }

    internal class DeleteCompanyClaimCommandComposer : Composer<DeleteCompanyClaimCommand>
    {
        public DeleteCompanyClaimCommandComposer(
            ITokenAccessor tokenAccessor,
            ICompanyClaimRepository companyClaimRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.Claim).LoadWith(ctx => companyClaimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}