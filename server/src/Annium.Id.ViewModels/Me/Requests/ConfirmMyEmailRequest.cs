using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Me;

namespace Annium.Id.ViewModels.Me.Requests
{
    public class ConfirmMyEmailRequest : ConfirmMyEmailRequestBase, IRequest<ConfirmMyEmailCommand>
    {
        public Guid AppId { get; set; }
    }

    public class ConfirmMyEmailRequestBase
    {
        public Guid Id { get; set; }
    }
}