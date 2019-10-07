using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Login
{
    public class LogInCommand : ICommand
    {
        public string AppKey { get; }
        public string Login { get; }
        public string Password { get; }
        public App App { get; private set; } = null!;
        public User User { get; private set; } = null!;

        public LogInCommand(
            string appKey,
            string login,
            string password
        )
        {
            AppKey = appKey;
            Login = login;
            Password = password;
        }
    }

    internal class LogInCommandValidator : Validator<LogInCommand>
    {
        public LogInCommandValidator()
        {
            Field(e => e.AppKey).Required().Length(2, 100);
            Field(e => e.Login).Required().Length(3, 50);
            Field(e => e.Password).Required().Length(8, 50);
        }
    }

    internal class LogInCommandComposer : Composer<LogInCommand>
    {
        public LogInCommandComposer(
            IAppRepository appRepository,
            IUserRepository userRepository
        )
        {
            Field(c => c.App).LoadWith(ctx => appRepository.FindByKeyAsync(ctx.Root.AppKey));
            Field(c => c.User).LoadWith(ctx => userRepository.FindByLoginAsync(ctx.Root.Login));
        }
    }
}