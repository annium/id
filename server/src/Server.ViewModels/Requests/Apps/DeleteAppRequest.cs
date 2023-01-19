using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Apps;

namespace Server.ViewModels.Requests.Apps;

public record DeleteAppRequest : IRequest<DeleteAppCommand>
{
    public Guid AppId { get; set; }
}