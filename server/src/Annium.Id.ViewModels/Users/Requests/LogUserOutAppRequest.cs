using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Users;

namespace Annium.Id.ViewModels.Users.Requests
{
    public class LogUserOutAppRequest : IRequest<LogUserOutAppCommand>
    {
        public Guid AppId { get; set; }
    }
}