using System;
using Annium.Architecture.ViewModel;
using NodaTime;
using Server.Domain.Models;

namespace Server.ViewModels.Responses.Login;

public record TokensResponse : IResponse<Tokens>
{
    public string AccessToken { get; set; } = string.Empty;
    public Guid RefreshToken { get; set; }
    public Instant RefreshTokenExpires { get; set; }
}
