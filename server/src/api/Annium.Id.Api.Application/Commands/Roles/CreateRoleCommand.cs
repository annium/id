using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Roles
{
    public class CreateRoleCommand : ICommand
    {
        public Guid AppId { get; }
        public string Key { get; }
        public string Name { get; }
        public Guid MyId { get; private set; }
        public App App { get; private set; } = null!;

        public CreateRoleCommand(
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

    internal class CreateRoleCommandValidator : Validator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator(
            IRoleRepository roleRepository
        )
        {
            Field(c => c.AppId).Required();
            Field(c => c.Key).Required().Length(3, 100).Then()
                .Unique(async(c, key) => await roleRepository.FindByKeyAsync(c.AppId, key) != null, "Role with {1} {2} already exists");
            Field(c => c.Name).Required().Length(3, 100);
        }
    }

    internal class CreateRoleCommandComposer : Composer<CreateRoleCommand>
    {
        public CreateRoleCommandComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
        }
    }
}