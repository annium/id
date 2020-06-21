using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.CompanyUsers
{
    public class AddCompanyClaimToCompanyUserCommand : ICommand
    {
        public Guid CompanyId { get; }
        public Guid UserId { get; }
        public Guid ClaimId { get; }
        public string Value { get; }
        public Guid MyId { get; private set; }
        public Company Company { get; private set; } = null!;
        public User User { get; private set; } = null!;
        public CompanyClaim Claim { get; private set; } = null!;

        public AddCompanyClaimToCompanyUserCommand(
            Guid companyId,
            Guid userId,
            Guid claimId,
            string value
        )
        {
            CompanyId = companyId;
            UserId = userId;
            ClaimId = claimId;
            Value = value;
        }
    }

    internal class AddCompanyClaimToCompanyUserCommandValidator : Validator<AddCompanyClaimToCompanyUserCommand>
    {
        public AddCompanyClaimToCompanyUserCommandValidator()
        {
            Field(c => c.CompanyId).Required();
            Field(c => c.UserId).Required();
            Field(c => c.ClaimId).Required();
            Field(c => c.Value).Required().Length(3, 100);
        }
    }

    internal class AddCompanyClaimToCompanyUserCommandComposer : Composer<AddCompanyClaimToCompanyUserCommand>
    {
        public AddCompanyClaimToCompanyUserCommandComposer(
            ITokenAccessor tokenAccessor,
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            ICompanyClaimRepository claimRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.Company).LoadWith(ctx => companyRepository.GetByIdAsync(ctx.Root.CompanyId));
            Field(c => c.User).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.UserId));
            Field(c => c.Claim).LoadWith(ctx => claimRepository.GetByIdAsync(ctx.Root.ClaimId));
        }
    }
}