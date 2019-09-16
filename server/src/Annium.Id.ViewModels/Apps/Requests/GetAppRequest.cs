using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Queries.Apps;

namespace Annium.Id.ViewModels.Apps.Requests
{
    public class GetAppRequest : IRequest<GetAppQuery>
    {
        public Guid AppId { get; set; }
    }
}