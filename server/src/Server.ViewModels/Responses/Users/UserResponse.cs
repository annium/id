using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Models;

namespace Server.ViewModels.Responses.Users;

public record UserResponse : IResponse<User>
{
    public Guid Id { get; set; }
    public string Login { get; set; } = string.Empty;
}