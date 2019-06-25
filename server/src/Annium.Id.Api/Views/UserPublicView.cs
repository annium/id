using System;

namespace Annium.Id.Api.Views
{
    public class UserPublicView
    {
        public Guid Id { get; }
        public string Login { get; }

        public UserPublicView(
            Guid id,
            string login
        )
        {
            Id = id;
            Login = login;
        }
    }
}