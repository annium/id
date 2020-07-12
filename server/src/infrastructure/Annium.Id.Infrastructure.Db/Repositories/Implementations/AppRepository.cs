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
        private readonly IContext _context;
        private readonly IMapper _mapper;

        public AppRepository(
            IContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<App> CreateAsync(App app)
        {
            var entity = _mapper.Map<Entities.App>(app);

            _context.Apps.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<App>(entity);
        }

        public async Task<App[]> FindAllAsync()
        {
            var apps = await _context.Apps.AsNoTracking()
                .ToListAsync();

            return apps.Select(_mapper.Map<App>).ToArray();
        }

        public async Task<App[]> FindMyAsync(Guid ownerId)
        {
            var apps = await _context.Apps.AsNoTracking()
                .Where(x => x.OwnerId == ownerId)
                .ToListAsync();

            return apps.Select(_mapper.Map<App>).ToArray();
        }

        public async Task<App> GetByIdAsync(Guid id)
        {
            var app = await _context.Apps.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<App>(app);
        }

        public async Task<App> FindByApiTokenAsync(Guid token)
        {
            var app = await _context.Apps.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ApiToken == token);

            return _mapper.Map<App>(app);
        }

        public async Task<App> UpdateAsync(App app)
        {
            var entity = await _context.Apps
                .SingleAsync(x => x.Id == app.Id);

            entity.OwnerId = app.OwnerId;
            entity.Name = app.Name;

            await _context.SaveChangesAsync();

            return _mapper.Map<App>(entity);
        }

        public async Task UpdateApiTokenAsync(Guid appId, Guid apiToken)
        {
            var entity = await _context.Apps
                .SingleAsync(x => x.Id == appId);

            entity.ApiToken = apiToken;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await _context.Apps
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
                return;

            _context.Apps.Remove(entity);

            await _context.SaveChangesAsync();
        }
    }
}