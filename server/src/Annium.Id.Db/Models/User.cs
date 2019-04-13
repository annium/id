using System;

namespace Annium.Id.Db
{
    public class User
    {
        public Guid Id { get; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public User(
            string login,
            string passwordHash,
            string firstName,
            string lastName,
            string email
        )
        {
            Login = login;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        internal User(
            Guid id,
            string login,
            string passwordHash,
            string firstName,
            string lastName,
            string email
        ) : this(login, passwordHash, firstName, lastName, email)
        {
            Id = id;
        }
    }
}