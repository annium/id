using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Login;

namespace Annium.Id.Api.ViewModels.Login.Requests
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