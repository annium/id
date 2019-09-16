using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Queries.Apps;

namespace Annium.Id.ViewModels.Apps.Requests
{
    public class GetAppApiTokenRequest : IRequest<GetAppApiTokenQuery>
    {
        public Guid AppId { get; set; }
    }
}