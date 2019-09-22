using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Login;

namespace Annium.Id.ViewModels.Login.Requests
{
    public class UpdateTokensRequest : IRequest<UpdateTokensCommand>
    {
        public Guid RefreshToken { get; set; }
    }
}