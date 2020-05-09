using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.CompanyUsers;

namespace Annium.Id.Api.ViewModels.CompanyUsers.Requests
{
    public class DeleteUserFromCompanyRequest : IRequest<DeleteUserFromCompanyCommand>
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
    }
}