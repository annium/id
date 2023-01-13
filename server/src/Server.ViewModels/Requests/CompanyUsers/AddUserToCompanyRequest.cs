using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.CompanyUsers;

namespace Server.ViewModels.Requests.CompanyUsers;

public class AddUserToCompanyRequest : IRequest<AddUserToCompanyCommand>
{
    public Guid CompanyId { get; set; }
    public Guid UserId { get; set; }
}