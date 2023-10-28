using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Me;

namespace Server.ViewModels.Requests.Me;

public record ConfirmMyEmailRequest : ConfirmMyEmailRequestBody, IRequest<ConfirmMyEmailCommand>
{
    public Guid AppId { get; set; }
}

public record ConfirmMyEmailRequestBody
{
    public Guid Id { get; set; }
}
