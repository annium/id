using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Companies;

namespace Server.ViewModels.Requests.Companies;

public record SetCompanyOwnerRequest : IRequest<SetCompanyOwnerCommand>
{
    public Guid CompanyId { get; set; }
    public Guid UserId { get; set; }
}
