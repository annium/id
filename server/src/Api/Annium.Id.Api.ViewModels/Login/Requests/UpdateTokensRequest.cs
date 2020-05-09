using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Login;

namespace Annium.Id.ViewModels.Login.Requests
{
    public class UpdateTokensRequest : UpdateTokensRequestBase, IRequest<UpdateTokensCommand>
    {
        public Guid AppId { get; set; }
    }

    public class UpdateTokensRequestBase
    {
        public Guid RefreshToken { get; set; }
    }
}