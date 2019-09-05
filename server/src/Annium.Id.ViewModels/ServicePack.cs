using System;
using Annium.Core.DependencyInjection;
using Annium.Core.Mapper;
using Annium.Id.Application.Queries.Users;
using Annium.Id.ViewModels.User.Requests;
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
            cfg.Map<GetUserProfileRequest, GetUserProfileQuery>().Ignore(t => t.User);
        }
    }
}