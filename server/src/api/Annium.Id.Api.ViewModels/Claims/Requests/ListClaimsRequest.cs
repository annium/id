using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Queries.Claims;

namespace Annium.Id.Api.ViewModels.Claims.Requests
{
    public class ListClaimsRequest : IRequest<ListClaimsQuery>
    {
        public Guid AppId { get; set; }
    }
}