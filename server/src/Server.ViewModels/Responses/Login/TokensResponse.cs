using System;
using Annium.Architecture.ViewModel;
using Core.Domain.Entities;
using NodaTime;

namespace Server.ViewModels.Responses.Login;

public class TokensResponse : IResponse<Tokens>
{
    public string AccessToken { get; set; } = string.Empty;
    public Guid RefreshToken { get; set; }
    public Instant RefreshTokenExpires { get; set; }
}