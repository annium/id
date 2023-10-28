using System;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Server.Db.Repositories;
using Server.Domain.Commands.Me;

namespace Server.Application.Commands.Me;

internal class RegisterMeCommandValidator : Validator<RegisterMeCommand>
{
    public RegisterMeCommandValidator(IUserRepository userRepository)
    {
        Field(e => e.Server).Required().Must(x => Uri.TryCreate(x, UriKind.Absolute, out _), "Server Uri is not valid");
        Field(e => e.Login)
            .Required()
            .Length(3, 50)
            .Then()
            .Unique(
                async (_, login) => await userRepository.TryFindByLoginAsync(login) != null,
                "User with {1} {2} already exists"
            );
        Field(e => e.Email)
            .Required()
            .Length(3, 100)
            .Email()
            .Then()
            .Unique(
                async (_, email) => await userRepository.TryFindByEmailAsync(email) != null,
                "User with {1} {2} already exists"
            );
    }
}

internal class RegisterMeCommandComposer : Composer<RegisterMeCommand>
{
    public RegisterMeCommandComposer(IUserRepository userRepository)
    {
        Field(e => e.ServerUri).LoadWith(ctx => new Uri(ctx.Root.Server));
        Field(e => e.Referral)
            .When(ctx => ctx.Root.ReferralId.HasValue)
            .LoadWith(async ctx => await userRepository.TryGetByIdAsync(ctx.Root.ReferralId!.Value));
    }
}
