using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.CompanyUsers;

namespace Annium.Id.Api.ViewModels.Requests.CompanyUsers
{
    public class DeleteUserFromCompanyRequest : IRequest<DeleteUserFromCompanyCommand>
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
    }
}