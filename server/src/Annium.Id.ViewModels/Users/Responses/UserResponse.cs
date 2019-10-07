using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;

namespace Annium.Id.ViewModels.Users.Responses
{
    public class UserResponse : IResponse<User>
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
    }
}