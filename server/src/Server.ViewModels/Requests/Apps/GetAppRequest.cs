using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Queries.Apps;

namespace Server.ViewModels.Requests.Apps;

public class GetAppRequest : IRequest<GetAppQuery>
{
    public Guid AppId { get; set; }
}