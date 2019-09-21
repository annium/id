using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.CompanyClaims
{
    public class UpdateCompanyClaimCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid ClaimId { get; }
        public string Key { get; }
        public string Name { get; }
        public Guid MyId { get; private set; }
        public App App { get; private set; }
        public CompanyClaim Claim { get; private set; }

        public UpdateCompanyClaimCommand(
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

    internal class UpdateCompanyClaimCommandValidator : Validator<UpdateCompanyClaimCommand>
    {
        public UpdateCompanyClaimCommandValidator()
        {
            Field(c => c.AppId).Required();
            Field(c => c.ClaimId).Required();
            Field(c => c.Key).Required().Length(3, 100);
            Field(c => c.Name).Required().Length(3, 100);
        }
    }

    internal class UpdateCompanyClaimCommandComposer : Composer<UpdateCompanyClaimCommand>
    {
        public UpdateCompanyClaimCommandComposer(
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