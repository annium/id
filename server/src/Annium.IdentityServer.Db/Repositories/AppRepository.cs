using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinqToDB;

namespace Annium.IdentityServer.Db
{
    internal class AppRepository : IAppRepository
    {
        private readonly IContext context;

        private readonly IMapper mapper;

        public AppRepository(
            IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<App> CreateAsync(App app)
        {
            var entity = mapper.Map<Entities.App>(app);
            entity.Id = Guid.NewGuid();

            using(var db = context.GetDataConnection())
            {
                await db.InsertAsync(entity);
            }

            return mapper.Map<App>(entity);
        }

        public async Task<App[]> GetAllAsync()
        {
            var apps = await context.Apps.ToArrayAsync();

            return apps.Select(mapper.Map<App>).ToArray();
        }

        public async Task<App> GetByIdAsync(Guid id)
        {
            var app = await context.Apps
                .FirstOrDefaultAsync(u => u.Id == id);

            return mapper.Map<App>(app);
        }

        public async Task<App> FindByNameAsync(string name)
        {
            var app = await context.Apps
                .FirstOrDefaultAsync(u => u.Login == name);

            return mapper.Map<App>(app);
        }

        public async Task<App> FindByApiTokenAsync(Guid token)
        {
            var app = await context.Apps.FirstOrDefaultAsync(u => u.ApiToken == token);

            return mapper.Map<App>(app);
        }

        public Task UpdateAsync(App app)
        {
            var entity = mapper.Map<Entities.App>(app);

            return context.Apps
                .UpdateAsync(
                    u => u.Id == entity.Id,
                    u => new Entities.App
                    {
                        Login = entity.Login,
                            PasswordHash = entity.PasswordHash,
                            ApiToken = entity.ApiToken,
                            Name = entity.Name,
                            Email = entity.Email,
                    }
                );
        }

        public Task UpdateApiTokenAsync(Guid appId, Guid apiToken)
        {
            return context.Apps
                .UpdateAsync(
                    u => u.Id == appId,
                    u => new Entities.App { ApiToken = apiToken, }
                );
        }

        public Task DeleteByIdAsync(Guid id)
        {
            return context.Apps.DeleteAsync(u => u.Id == id);
        }
    }
}