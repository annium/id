using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Login;

namespace Annium.Id.ViewModels.Login.Requests
{
    public class LogUserOutAppRequest : IRequest<LogUserOutAppCommand>
    {
        public Guid AppId { get; set; }
    }
}