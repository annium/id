using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.Companies;

namespace Annium.Id.Api.ViewModels.Requests.Companies
{
    public class SetCompanyOwnerRequest : IRequest<SetCompanyOwnerCommand>
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
    }
}