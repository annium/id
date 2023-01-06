using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Queries.CompanyClaims;

namespace Annium.Id.Api.ViewModels.Requests.CompanyClaims;

public class ListCompanyClaimsRequest : IRequest<ListCompanyClaimsQuery>
{
    public Guid AppId { get; set; }
}