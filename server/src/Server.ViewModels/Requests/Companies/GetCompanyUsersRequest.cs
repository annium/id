using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Queries.Companies;

namespace Server.ViewModels.Requests.Companies;

public class GetCompanyUsersRequest : IRequest<GetCompanyUsersQuery>
{
    public Guid CompanyId { get; set; }
}