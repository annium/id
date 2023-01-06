using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Queries.Apps;

namespace Annium.Id.Api.ViewModels.Requests.Apps;

public class GetAppApiTokenRequest : IRequest<GetAppApiTokenQuery>
{
    public Guid AppId { get; set; }
}