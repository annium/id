using System;
using Annium.Core.DependencyInjection;
using Annium.Core.Mapper;
using Annium.Id.Application.Commands.Users;
using Annium.Id.Application.Queries.Users;
using Annium.Id.ViewModels.Users.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.ViewModels
{
    public class ServicePack : ServicePackBase
    {
        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddMapperConfiguration(ConfigureMapping);
        }

        private void ConfigureMapping(MapperConfiguration cfg)
        {
            #region Users
            cfg.Map<DeleteUserRequest, DeleteUserCommand>().Ignore(t => t.UserId);
            cfg.Map<GetUserRequest, GetUserQuery>().Ignore(t => t.User);
            cfg.Map<LogUserOutRequest, LogUserOutCommand>().Ignore(t => t.LoginId);
            #endregion
        }
    }
}