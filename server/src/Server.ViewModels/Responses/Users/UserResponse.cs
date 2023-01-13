using System;
using Annium.Architecture.ViewModel;
using Core.Domain.Entities;

namespace Server.ViewModels.Responses.Users;

public class UserResponse : IResponse<User>
{
    public Guid Id { get; set; }
    public string Login { get; set; } = string.Empty;
}