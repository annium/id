using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Users;

namespace Annium.Id.ViewModels.Users.Requests
{
    public class LogUserInAppRequest : IRequest<LogUserInAppCommand>
    {
        public Guid AppId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}