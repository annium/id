using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Queries.Apps;

namespace Server.ViewModels.Requests.Apps;

public class GetAppApiTokenRequest : IRequest<GetAppApiTokenQuery>
{
    public Guid AppId { get; set; }
}