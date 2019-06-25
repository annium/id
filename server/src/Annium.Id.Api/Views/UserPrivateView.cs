using System;

namespace Annium.Id.Api.Views
{
    public class UserPrivateView
    {
        public Guid Id { get; }
        public string Login { get; }
        public string Email { get; }

        public UserPrivateView(
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