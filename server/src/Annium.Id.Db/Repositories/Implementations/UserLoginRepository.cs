using System;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using LinqToDB;
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
            entity.Id = Guid.NewGuid();

            using(var db = context.GetDataConnection())
            {
                await db.InsertAsync(entity);
            }

            return mapper.Map<UserLogin>(entity);
        }

        public async Task<UserLogin> FindByRefreshTokenAsync(Guid token)
        {
            var entity = await context.UserLogins
                .FirstOrDefaultAsync(l => l.RefreshToken == token);

            return mapper.Map<UserLogin>(entity);
        }

        public async Task<UserLogin> UpdateRefreshTokenAsync(UserLogin login)
        {
            var entity = mapper.Map<Entities.UserLogin>(login);

            await context.UserLogins
                .UpdateAsync(
                    l => l.Id == login.Id,
                    l => new Entities.UserLogin
                    {
                        RefreshToken = entity.RefreshToken,
                            RefreshTokenExpires = entity.RefreshTokenExpires,
                    }
                );

            return mapper.Map<UserLogin>(entity);
        }

        public Task DeleteByIdAsync(Guid id)
        {
            return context.UserLogins.DeleteAsync(l => l.Id == id);
        }

        public Task DeleteExpiredByUserIdAsync(Guid userId, Instant instant)
        {
            var instantTime = instant.ToDateTimeUtc();

            return context.UserLogins.DeleteAsync(l => l.UserId == userId && l.RefreshTokenExpires <= instantTime);
        }

        public Task DeleteAllByUserIdAsync(Guid userId)
        {
            return context.UserLogins.DeleteAsync(l => l.UserId == userId);
        }
    }
}