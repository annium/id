using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Companies;

namespace Annium.Id.Api.ViewModels.Companies.Requests
{
    public class SetCompanyOwnerRequest : IRequest<SetCompanyOwnerCommand>
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
    }
}