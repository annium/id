using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Queries.CompanyClaims;

namespace Annium.Id.Api.ViewModels.CompanyClaims.Requests
{
    public class ListCompanyClaimsRequest : IRequest<ListCompanyClaimsQuery>
    {
        public Guid AppId { get; set; }
    }
}