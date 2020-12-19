using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Queries.Claims;

namespace Annium.Id.Api.ViewModels.Requests.Claims
{
    public class ListClaimsRequest : IRequest<ListClaimsQuery>
    {
        public Guid AppId { get; set; }
    }
}