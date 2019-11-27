using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Me;

namespace Annium.Id.ViewModels.Me.Requests
{
    public class ConfirmMyEmailRequest : ConfirmMyEmailRequestBase, IRequest<ConfirmMyEmailCommand>
    {
        public string AppKey { get; set; } = string.Empty;
    }

    public class ConfirmMyEmailRequestBase
    {
        public Guid Id { get; set; }
    }
}