using System;
using Annium.Architecture.CQRS.Commands;

namespace Annium.Id.Api.Domain.Commands.Companies
{
    public class RegisterCompanyCommand : ICommand
    {
        public Guid? ParentId { get; }
        public string Name { get; }
        public Guid MyId { get; private set; }

        public RegisterCompanyCommand(
            Guid? parentId,
            string name
        )
        {
            ParentId = parentId;
            Name = name;
        }
    }
}