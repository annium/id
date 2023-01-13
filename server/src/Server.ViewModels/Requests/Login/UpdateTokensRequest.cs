using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Login;

namespace Server.ViewModels.Requests.Login;

public class UpdateTokensRequest : UpdateTokensRequestBody, IRequest<UpdateTokensCommand>
{
    public Guid AppId { get; set; }
}

public class UpdateTokensRequestBody
{
    public Guid RefreshToken { get; set; }
}