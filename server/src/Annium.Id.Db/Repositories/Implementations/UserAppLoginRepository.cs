using System;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using LinqToDB;
using NodaTime;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class UserAppLoginRepository : IUserAppLoginRepository
    {
        private readonly Entities.IContext context;
        private readonly IMapper mapper;

        public UserAppLoginRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<UserAppLogin> CreateAsync(UserAppLogin login)
        {
            var entity = mapper.Map<Entities.UserAppLogin>(login);
            entity.Id = Guid.NewGuid();

            using(var db = context.GetDataConnection())
            {
                await db.InsertAsync(entity);
            }

            return mapper.Map<UserAppLogin>(entity);
        }

        public async Task<UserAppLogin> FindByRefreshTokenAsync(Guid token)
        {
            var entity = await context.UserAppLogins
                .FirstOrDefaultAsync(l => l.RefreshToken == token);

            return mapper.Map<UserAppLogin>(entity);
        }

        public async Task<UserAppLogin> UpdateRefreshTokenAsync(UserAppLogin login)
        {
            var entity = mapper.Map<Entities.UserAppLogin>(login);

            await context.UserAppLogins
                .UpdateAsync(
                    l => l.Id == login.Id,
                    l => new Entities.UserAppLogin
                    {
                        RefreshToken = entity.RefreshToken,
                            RefreshTokenExpires = entity.RefreshTokenExpires,
                    }
                );

            return mapper.Map<UserAppLogin>(entity);
        }

        public Task DeleteByIdAsync(Guid id)
        {
            return context.UserAppLogins.DeleteAsync(l => l.Id == id);
        }

        public Task DeleteExpiredByUserIdAsync(Guid userId, Instant instant)
        {
            var instantTime = instant.ToDateTimeUtc();

            return context.UserAppLogins.DeleteAsync(l => l.UserId == userId && l.RefreshTokenExpires <= instantTime);
        }

        public Task DeleteAllByUserIdAsync(Guid userId)
        {
            return context.UserAppLogins.DeleteAsync(l => l.UserId == userId);
        }
    }
}