using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.CompanyUsers;

namespace Annium.Id.ViewModels.CompanyUsers.Requests
{
    public class DeleteCompanyRoleFromCompanyUserRequest : IRequest<DeleteCompanyRoleFromCompanyUserCommand>
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}