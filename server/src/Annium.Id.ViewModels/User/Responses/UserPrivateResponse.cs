using System;

namespace Annium.Id.ViewModels.User.Responses
{
    public class UserPrivateResponse
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