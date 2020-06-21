using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Me
{
    public class RegisterMeCommand : ICommand
    {
        public string Server { get; }
        public string Email { get; }
        public string Login { get; }
        public Guid? ReferralId { get; }
        public Uri ServerUri { get; private set; } = default!;
        public User? Referral { get; private set; } = default!;

        public RegisterMeCommand(
            string server,
            string email,
            string login,
            Guid? referralId
        )
        {
            Server = server;
            Email = email;
            Login = login;
            ReferralId = referralId;
        }
    }

    internal class RegisterMeCommandValidator : Validator<RegisterMeCommand>
    {
        public RegisterMeCommandValidator(
            IUserRepository userRepository
        )
        {
            Field(e => e.Server).Required().Must(x => Uri.TryCreate(x, UriKind.Absolute, out _), "Server Uri is not valid");
            Field(e => e.Login).Required().Length(3, 50).Then()
                .Unique(async (c, login) => await userRepository.FindByLoginAsync(login) != null, "User with {1} {2} already exists");
            Field(e => e.Email).Required().Length(3, 100).Email().Then()
                .Unique(async (c, email) => await userRepository.FindByEmailAsync(email) != null, "User with {1} {2} already exists");
        }
    }

    internal class RegisterMeCommandComposer : Composer<RegisterMeCommand>
    {
        public RegisterMeCommandComposer(
            IUserRepository userRepository
        )
        {
            Field(e => e.ServerUri).LoadWith(ctx => new Uri(ctx.Root.Server));
            Field(e => e.Referral)
                .When(ctx => ctx.Root.ReferralId.HasValue)
                .LoadWith(async ctx => await userRepository.GetByIdAsync(ctx.Root.ReferralId!.Value));
        }
    }
}