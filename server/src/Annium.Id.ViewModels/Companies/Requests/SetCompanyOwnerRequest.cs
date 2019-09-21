using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Companies;

namespace Annium.Id.ViewModels.Companies.Requests
{
    public class SetCompanyOwnerRequest : IRequest<SetCompanyOwnerCommand>
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
    }
}