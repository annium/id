using System;

namespace Annium.Id.Domain.Entities
{
    public class User
    {
        public Guid Id { get; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        public User(
            string login,
            string passwordHash,
            string email
        )
        {
            Login = login;
            PasswordHash = passwordHash;
            Email = email;
        }

        internal User(
            Guid id,
            string login,
            string passwordHash,
            string email
        ) : this(login, passwordHash, email)
        {
            Id = id;
        }
    }
}