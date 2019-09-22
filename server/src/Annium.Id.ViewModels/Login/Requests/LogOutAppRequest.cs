using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Login;

namespace Annium.Id.ViewModels.Login.Requests
{
    public class LogOutAppRequest : IRequest<LogOutAppCommand>
    {
        public Guid AppId { get; set; }
    }
}