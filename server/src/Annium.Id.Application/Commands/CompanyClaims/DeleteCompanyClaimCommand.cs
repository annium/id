using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.CompanyClaims
{
    public class DeleteCompanyClaimCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid ClaimId { get; }
        public Guid MyId { get; private set; }
        public App App { get; private set; }
        public CompanyClaim Claim { get; private set; }

        public DeleteCompanyClaimCommand(
            Guid appId,
            Guid claimId
        )
        {
            AppId = appId;
            ClaimId = claimId;
        }
    }

    internal class DeleteCompanyClaimCommandValidator : Validator<DeleteCompanyClaimCommand>
    {
        public DeleteCompanyClaimCommandValidator()
        {
            Field(c => c.AppId).Required();
            Field(c => c.ClaimId).Required();
        }
    }

    internal class DeleteCompanyClaimCommandComposer : Composer<DeleteCompanyClaimCommand>
    {
        public DeleteCompanyClaimCommandComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository,
            ICompanyClaimRepository companyClaimRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(c => c.Claim).LoadWith(ctx => companyClaimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}