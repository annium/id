using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Infrastructure.Db.Repositories.Implementations
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

            context.Apps.Add(entity);
            await context.SaveChangesAsync();

            return mapper.Map<App>(entity);
        }

        public async Task<App[]> GetAllAsync()
        {
            var apps = await context.Apps.AsNoTracking().ToListAsync();

            return apps.Select(mapper.Map<App>).ToArray();
        }

        public async Task<App> GetByIdAsync(Guid id)
        {
            var app = await context.Apps.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<App>(app);
        }

        public async Task<App> FindByApiTokenAsync(Guid token)
        {
            var app = await context.Apps.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ApiToken == token);

            return mapper.Map<App>(app);
        }

        public async Task<App> UpdateAsync(App app)
        {
            var entity = await context.Apps
                .SingleAsync(x => x.Id == app.Id);

            entity.OwnerId = app.OwnerId;
            entity.Name = app.Name;

            await context.SaveChangesAsync();

            return mapper.Map<App>(entity);
        }

        public async Task UpdateApiTokenAsync(Guid appId, Guid apiToken)
        {
            var entity = await context.Apps
                .SingleAsync(x => x.Id == appId);

            entity.ApiToken = apiToken;

            await context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.Apps
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
                return;

            context.Apps.Remove(entity);

            await context.SaveChangesAsync();
        }
    }
}