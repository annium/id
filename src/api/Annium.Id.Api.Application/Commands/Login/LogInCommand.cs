using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Login
{
    public class LogInCommand : ICommand
    {
        public Guid AppId { get; }
        public string Login { get; }
        public string Password { get; }
        public App App { get; private set; } = null!;
        public User User { get; private set; } = null!;

        public LogInCommand(
            Guid appId,
            string login,
            string password
        )
        {
            AppId = appId;
            Login = login;
            Password = password;
        }
    }

    internal class LogInCommandValidator : Validator<LogInCommand>
    {
        public LogInCommandValidator()
        {
            Field(e => e.AppId).Required();
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
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(c => c.User).LoadWith(ctx => userRepository.FindByLoginAsync(ctx.Root.Login));
        }
    }
}