using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Queries.Apps;

namespace Annium.Id.Api.ViewModels.Apps.Requests
{
    public class GetAppApiTokenRequest : IRequest<GetAppApiTokenQuery>
    {
        public Guid AppId { get; set; }
    }
}