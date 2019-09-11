using System;

namespace Annium.Id.ViewModels.Users.Responses
{
    public class UserPublicResponse
    {
        public Guid Id { get; }
        public string Login { get; }

        public UserPublicResponse(
            Guid id,
            string login
        )
        {
            Id = id;
            Login = login;
        }
    }
}