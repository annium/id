using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.CompanyClaims
{
    public class CreateCompanyClaimCommand : ICommand
    {
        public Guid AppId { get; }
        public string Key { get; }
        public string Name { get; }
        public Guid MyId { get; private set; }
        public App App { get; private set; }

        public CreateCompanyClaimCommand(
            Guid appId,
            string key,
            string name
        )
        {
            AppId = appId;
            Key = key;
            Name = name;
        }
    }

    internal class CreateCompanyClaimCommandValidator : Validator<CreateCompanyClaimCommand>
    {
        public CreateCompanyClaimCommandValidator(
            ICompanyClaimRepository companyClaimRepository
        )
        {
            Field(c => c.AppId).NotEqual(Guid.Empty);
            Field(c => c.Key).Required().Length(3, 100).Then()
                .Unique(async(c, key) => await companyClaimRepository.FindByKeyAsync(c.AppId, key) != null, "Company claim with {1} {2} already exists");
            Field(c => c.Name).Required().Length(3, 100);
        }
    }

    internal class CreateCompanyClaimCommandComposer : Composer<CreateCompanyClaimCommand>
    {
        public CreateCompanyClaimCommandComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
        }
    }
}