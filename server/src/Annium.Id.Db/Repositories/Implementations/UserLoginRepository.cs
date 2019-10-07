using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class UserLoginRepository : IUserLoginRepository
    {
        private readonly Entities.IContext context;
        private readonly IMapper mapper;

        public UserLoginRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<UserLogin> CreateAsync(UserLogin login)
        {
            var entity = mapper.Map<Entities.UserLogin>(login);

            context.UserLogins.Add(entity);
            await context.SaveChangesAsync();

            return mapper.Map<UserLogin>(entity);
        }

        public async Task<UserLogin> FindByRefreshTokenAsync(Guid token)
        {
            var entity = await context.UserLogins.AsNoTracking()
                .FirstOrDefaultAsync(x => x.RefreshToken == token);

            return mapper.Map<UserLogin>(entity);
        }

        public async Task<UserLogin> UpdateRefreshTokenAsync(UserLogin login)
        {
            var entity = await context.UserLogins
                .SingleAsync(x => x.Id == login.Id);

            entity.RefreshToken = login.RefreshToken;
            entity.RefreshTokenExpires = mapper.Map<DateTime>(login.RefreshTokenExpires);

            await context.SaveChangesAsync();

            return mapper.Map<UserLogin>(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.UserLogins
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
                return;

            context.UserLogins.Remove(entity);

            await context.SaveChangesAsync();
        }

        public async Task DeleteExpiredByUserIdAsync(Guid userId, Instant instant)
        {
            var expires = mapper.Map<DateTime>(instant);

            var entities = await context.UserLogins
                .Where(x => x.UserId == userId && x.RefreshTokenExpires <= expires)
                .ToListAsync();

            if (entities.Count == 0)
                return;

            context.UserLogins.RemoveRange(entities);

            await context.SaveChangesAsync();
        }

        public async Task DeleteAllByUserIdAsync(Guid userId)
        {
            var entities = await context.UserLogins
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (entities.Count == 0)
                return;

            context.UserLogins.RemoveRange(entities);

            await context.SaveChangesAsync();
        }
    }
}