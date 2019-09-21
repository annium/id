using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Queries.CompanyClaims;

namespace Annium.Id.ViewModels.CompanyClaims.Requests
{
    public class ListCompanyClaimsRequest : IRequest<ListCompanyClaimsQuery>
    {
        public Guid AppId { get; set; }
    }
}