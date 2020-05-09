using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;
using NodaTime;

namespace Annium.Id.ViewModels.Login.Responses
{
    public class TokensResponse : IResponse<Tokens>
    {
        public string AccessToken { get; set; } = string.Empty;
        public Guid RefreshToken { get; set; }
        public Instant RefreshTokenExpires { get; set; }
    }
}