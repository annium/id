using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Companies
{
    public class SetCompanyOwnerCommand : ICommand
    {
        public Guid CompanyId { get; }
        public Guid UserId { get; }
        public Guid MyId { get; private set; }
        public Company Company { get; private set; }
        public User User { get; private set; }

        public SetCompanyOwnerCommand(
            Guid companyId,
            Guid userId
        )
        {
            CompanyId = companyId;
            UserId = userId;
        }
    }

    internal class SetCompanyOwnerCommandValidator : Validator<SetCompanyOwnerCommand>
    {
        public SetCompanyOwnerCommandValidator()
        {
            Field(c => c.CompanyId).Required();
            Field(c => c.UserId).Required();
        }
    }

    internal class SetCompanyOwnerCommandComposer : Composer<SetCompanyOwnerCommand>
    {
        public SetCompanyOwnerCommandComposer(
            ITokenAccessor tokenAccessor,
            ICompanyRepository companyRepository,
            IUserRepository userRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.Company).LoadWith(ctx => companyRepository.GetByIdAsync(ctx.Root.CompanyId));
            Field(c => c.User).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.UserId));
        }
    }
}