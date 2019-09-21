using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.CompanyUsers;

namespace Annium.Id.ViewModels.CompanyUsers.Requests
{
    public class AddUserToCompanyRequest : IRequest<AddUserToCompanyCommand>
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
    }
}