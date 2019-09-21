using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;

namespace Annium.Id.ViewModels.Users.Responses
{
    public class UserPrivateResponse : IResponse<User>
    {
        public Guid Id { get; }
        public string Login { get; }
        public string Email { get; }

        public UserPrivateResponse(
            Guid id,
            string login,
            string email
        )
        {
            Id = id;
            Login = login;
            Email = email;
        }
    }
}