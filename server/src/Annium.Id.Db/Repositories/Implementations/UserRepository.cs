using System;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class UserRepository : IUserRepository
    {
        private readonly Entities.IContext context;
        private readonly IMapper mapper;

        public UserRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<User> CreateAsync(User user)
        {
            var entity = mapper.Map<Entities.User>(user);

            context.Users.Add(entity);
            await context.SaveChangesAsync();

            return mapper.Map<User>(entity);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var entity = await context.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<User>(entity);
        }

        public async Task<User> FindByLoginAsync(string login)
        {
            var entity = await context.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == login);

            return mapper.Map<User>(entity);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var entity = await context.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);

            return mapper.Map<User>(entity);
        }

        public async Task<User> UpdateAsync(User user)
        {
            var entity = await context.Users
                .FirstOrDefaultAsync(x => x.Id == user.Id);

            entity.Login = user.Login;
            entity.PasswordHash = user.PasswordHash;
            entity.Email = user.Email;

            await context.SaveChangesAsync();

            return mapper.Map<User>(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
                return;

            context.Users.Remove(entity);

            await context.SaveChangesAsync();
        }
    }
}