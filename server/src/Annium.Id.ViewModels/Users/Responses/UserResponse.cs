using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;

namespace Annium.Id.ViewModels.Users.Responses
{
    public class UserResponse : IResponse<User>
    {
        public Guid Id { get; }
        public string Login { get; }

        public UserResponse(
            Guid id,
            string login
        )
        {
            Id = id;
            Login = login;
        }
    }
}