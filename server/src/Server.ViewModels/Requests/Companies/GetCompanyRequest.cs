using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Queries.Companies;

namespace Server.ViewModels.Requests.Companies;

public record GetCompanyRequest : IRequest<GetCompanyQuery>
{
    public Guid CompanyId { get; set; }
}