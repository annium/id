using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.CompanyUsers;

namespace Annium.Id.Api.ViewModels.Requests.CompanyUsers
{
    public class DeleteCompanyClaimFromCompanyUserRequest : IRequest<DeleteCompanyClaimFromCompanyUserCommand>
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public Guid ClaimId { get; set; }
    }
}