using System;
using Annium.Core.DependencyInjection;
using Annium.Core.Mapper;
using Annium.Id.Application.Commands.Apps;
using Annium.Id.Application.Commands.Login;
using Annium.Id.Application.Commands.Me;
using Annium.Id.Application.Queries.Me;
using Annium.Id.ViewModels.Apps.Requests;
using Annium.Id.ViewModels.Login.Requests;
using Annium.Id.ViewModels.Me.Requests;
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
            // TODO: instead of ignores, use direct maps
            #region Users
            cfg.Map<UnregisterMeRequest, UnregisterMeCommand>().Ignore(t => t.MyId);
            cfg.Map<LogOutRequest, LogOutCommand>().Ignore(t => t.LoginId);
            cfg.Map<GetMeRequest, GetMeQuery>().Ignore(t => t.User);
            #endregion
            #region Apps
            cfg.Map<SetAppOwnerRequest, SetAppOwnerCommand>().Ignore(t => t.MyId);
            cfg.Map<LogOutRequest, LogOutCommand>().Ignore(t => t.LoginId);
            cfg.Map<GetMeRequest, GetMeQuery>().Ignore(t => t.User);
            #endregion
        }
    }
}