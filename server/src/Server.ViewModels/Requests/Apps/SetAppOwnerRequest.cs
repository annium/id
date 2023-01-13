using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Apps;

namespace Server.ViewModels.Requests.Apps;

public class SetAppOwnerRequest : IRequest<SetAppOwnerCommand>
{
    public Guid AppId { get; set; }
    public Guid NewOwnerId { get; set; }
}