using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Queries.Apps;

namespace Server.ViewModels.Requests.Apps;

public record GetAppApiTokenRequest : IRequest<GetAppApiTokenQuery>
{
    public Guid AppId { get; set; }
}
