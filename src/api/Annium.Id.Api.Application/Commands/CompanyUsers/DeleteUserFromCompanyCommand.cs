using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.CompanyUsers
{
    public class DeleteUserFromCompanyCommand : ICommand
    {
        public Guid CompanyId { get; }
        public Guid UserId { get; }
        public Guid MyId { get; private set; }
        public Company Company { get; private set; } = null!;
        public User User { get; private set; } = null!;

        public DeleteUserFromCompanyCommand(
            Guid companyId,
            Guid userId
        )
        {
            CompanyId = companyId;
            UserId = userId;
        }
    }

    internal class DeleteUserFromCompanyCommandValidator : Validator<DeleteUserFromCompanyCommand>
    {
        public DeleteUserFromCompanyCommandValidator()
        {
            Field(c => c.CompanyId).Required();
            Field(c => c.UserId).Required();
        }
    }

    internal class DeleteUserFromCompanyCommandComposer : Composer<DeleteUserFromCompanyCommand>
    {
        public DeleteUserFromCompanyCommandComposer(
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