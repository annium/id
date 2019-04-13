using System;
using Annium.Id.Db;
using Newtonsoft.Json;

namespace Annium.Id.Api.Views
{
    public class UserView
    {
        public Guid Id { get; }

        public string Login { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }

        public UserView(User user)
        {
            Id = user.Id;
            Login = user.Login;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
        }

        [JsonConstructor]
        public UserView(
            Guid id,
            string login,
            string firstName,
            string lastName,
            string email
        )
        {
            Id = id;
            Login = login;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}