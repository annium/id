using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;

namespace Annium.Id.ViewModels.Me.Responses
{
    public class MeResponse : IResponse<User>
    {
        public Guid Id { get; }
        public string Login { get; }
        public string Email { get; }

        public MeResponse(
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