using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.Me;

namespace Annium.Id.Api.ViewModels.Requests.Me
{
    public class ConfirmMyEmailRequest : ConfirmMyEmailRequestBody, IRequest<ConfirmMyEmailCommand>
    {
        public Guid AppId { get; set; }
    }

    public class ConfirmMyEmailRequestBody
    {
        public Guid Id { get; set; }
    }
}