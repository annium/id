using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Apps;

namespace Annium.Id.Api.ViewModels.Apps.Requests
{
    public class SetAppOwnerRequest : IRequest<SetAppOwnerCommand>
    {
        public Guid AppId { get; set; }
        public Guid NewOwnerId { get; set; }
    }
}