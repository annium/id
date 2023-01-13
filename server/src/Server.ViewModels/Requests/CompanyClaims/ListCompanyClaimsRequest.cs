using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Queries.CompanyClaims;

namespace Server.ViewModels.Requests.CompanyClaims;

public class ListCompanyClaimsRequest : IRequest<ListCompanyClaimsQuery>
{
    public Guid AppId { get; set; }
}