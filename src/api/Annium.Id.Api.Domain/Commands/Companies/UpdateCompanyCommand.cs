using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Commands.Companies
{
    public class UpdateCompanyCommand : ICommand
    {
        public Guid CompanyId { get; }
        public Guid? ParentId { get; }
        public string Name { get; }
        public Guid MyId { get; private set; }
        public Company Company { get; private set; } = null!;

        public UpdateCompanyCommand(
            Guid companyId,
            Guid? parentId,
            string name
        )
        {
            CompanyId = companyId;
            ParentId = parentId;
            Name = name;
        }
    }
}