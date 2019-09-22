using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Login;

namespace Annium.Id.ViewModels.Login.Requests
{
    public class LogUserInAppRequest : IRequest<LogUserInAppCommand>
    {
        public Guid AppId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}