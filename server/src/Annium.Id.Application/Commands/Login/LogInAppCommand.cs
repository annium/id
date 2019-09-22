using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Login
{
    public class LogInAppCommand : ICommand
    {
        public Guid AppId { get; }
        public string Login { get; }
        public string Password { get; }
        public App App { get; private set; }
        public User User { get; private set; }

        public LogInAppCommand(
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

    internal class LogInAppCommandValidator : Validator<LogInAppCommand>
    {
        public LogInAppCommandValidator()
        {
            Field(e => e.AppId).Required();
            Field(e => e.Login).Required().Length(3, 50);
            Field(e => e.Password).Required().Length(8, 50);
        }
    }

    internal class LogInAppCommandComposer : Composer<LogInAppCommand>
    {
        public LogInAppCommandComposer(
            IAppRepository appRepository,
            IUserRepository userRepository
        )
        {
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(c => c.User).LoadWith(ctx => userRepository.FindByLoginAsync(ctx.Root.Login));
        }
    }
}