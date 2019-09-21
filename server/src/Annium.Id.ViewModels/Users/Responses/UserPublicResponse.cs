using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;

namespace Annium.Id.ViewModels.Users.Responses
{
    public class UserPublicResponse : IResponse<User>
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