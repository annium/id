using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Queries.Claims;

namespace Annium.Id.ViewModels.Claims.Requests
{
    public class ListClaimsRequest : IRequest<ListClaimsQuery>
    {
        public Guid AppId { get; set; }
    }
}