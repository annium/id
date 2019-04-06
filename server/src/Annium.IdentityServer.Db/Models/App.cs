using System;

namespace Annium.IdentityServer.Db
{
    public class App
    {
        public Guid Id { get; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public Guid ApiToken { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public App(
            string login,
            string passwordHash,
            Guid apiToken,
            string name,
            string email
        )
        {
            Login = login;
            PasswordHash = passwordHash;
            ApiToken = apiToken;
            Name = name;
            Email = email;
        }

        internal App(
            Guid id,
            string login,
            string passwordHash,
            Guid apiToken,
            string name,
            string email
        ) : this(login, passwordHash, apiToken, name, email)
        {
            Id = id;
        }
    }
}