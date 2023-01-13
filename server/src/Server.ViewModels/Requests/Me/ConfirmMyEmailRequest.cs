using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Me;

namespace Server.ViewModels.Requests.Me;

public class ConfirmMyEmailRequest : ConfirmMyEmailRequestBody, IRequest<ConfirmMyEmailCommand>
{
    public Guid AppId { get; set; }
}

public class ConfirmMyEmailRequestBody
{
    public Guid Id { get; set; }
}