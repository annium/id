using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using LinqToDB;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class AppRepository : IAppRepository
    {
        private readonly Entities.IContext context;
        private readonly IMapper mapper;

        public AppRepository(
            Entities.IContext context,
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

        public async Task<App> FindByKeyAsync(string key)
        {
            var app = await context.Apps
                .FirstOrDefaultAsync(u => u.Key == key);

            return mapper.Map<App>(app);
        }

        public async Task<App> FindByApiTokenAsync(Guid token)
        {
            var app = await context.Apps.FirstOrDefaultAsync(u => u.ApiToken == token);

            return mapper.Map<App>(app);
        }

        public async Task<App> UpdateAsync(App app)
        {
            var entity = mapper.Map<Entities.App>(app);

            await context.Apps
                .UpdateAsync(
                    u => u.Id == entity.Id,
                    u => new Entities.App
                    {
                        Key = entity.Key,
                            Name = entity.Name,
                            OwnerId = entity.OwnerId,
                    }
                );

            return mapper.Map<App>(entity);
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