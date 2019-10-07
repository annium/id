using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Login;

namespace Annium.Id.ViewModels.Login.Requests
{
    public class UpdateTokensRequest : UpdateTokensRequestBase, IRequest<UpdateTokensCommand>
    {
        public string AppKey { get; set; } = string.Empty;
    }

    public class UpdateTokensRequestBase
    {
        public Guid RefreshToken { get; set; }
    }
}