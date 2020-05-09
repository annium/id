using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Me;

namespace Annium.Id.Api.ViewModels.Me.Requests
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