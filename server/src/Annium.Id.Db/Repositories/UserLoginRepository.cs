using System;
using System.Threading.Tasks;
using AutoMapper;
using LinqToDB;
using NodaTime;

namespace Annium.Id.Db
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

        public async Task<ValueTuple<Guid, Instant>> UpdateRefreshTokenAsync(Guid token, Instant expires)
        {
            var entity = await context.UserLogins
                .FirstOrDefaultAsync(l => l.RefreshToken == token);

            if (entity == null)
                return (Guid.Empty, Instant.MinValue);

            token = Guid.NewGuid();

            await context.UserLogins
                .UpdateAsync(
                    l => l.Id == entity.Id,
                    l => new Entities.UserLogin
                    {
                        RefreshToken = token,
                            RefreshTokenExpires = expires,
                    }
                );

            return (token, expires);
        }

        public Task DeleteByIdAsync(Guid id)
        {
            return context.UserLogins.DeleteAsync(l => l.Id == id);
        }

        public Task DeleteExpiredByUserIdAsync(Guid userId, Instant instant)
        {
            return context.UserLogins.DeleteAsync(l => l.UserId == userId && l.RefreshTokenExpires <= instant);
        }

        public Task DeleteAllByUserIdAsync(Guid userId)
        {
            return context.UserLogins.DeleteAsync(l => l.UserId == userId);
        }
    }
}