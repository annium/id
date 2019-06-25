using System;
using Annium.Id.Db;
using Newtonsoft.Json;

namespace Annium.Id.Api.Views
{
    public class UserView
    {
        public Guid Id { get; }

        public string Login { get; }

        public string Email { get; }

        public UserView(User user)
        {
            Id = user.Id;
            Login = user.Login;
            Email = user.Email;
        }

        [JsonConstructor]
        public UserView(
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