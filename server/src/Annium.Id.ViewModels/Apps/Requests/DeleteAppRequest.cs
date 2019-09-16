using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Apps;

namespace Annium.Id.ViewModels.Apps.Requests
{
    public class DeleteAppRequest : IRequest<DeleteAppCommand>
    {
        public Guid AppId { get; set; }
    }
}