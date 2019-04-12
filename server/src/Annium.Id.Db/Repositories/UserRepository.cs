using System;
using System.Threading.Tasks;
using AutoMapper;
using LinqToDB;

namespace Annium.Id.Db
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
            entity.Id = Guid.NewGuid();

            using(var db = context.GetDataConnection())
            {
                await db.InsertAsync(entity);
            }

            return mapper.Map<User>(entity);
        }

        public async Task<User> GetById(Guid id)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            return mapper.Map<User>(user);
        }

        public async Task<User> FindByLoginAsync(string login)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Login == login);

            return mapper.Map<User>(user);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);

            return mapper.Map<User>(user);
        }

        public Task UpdateAsync(User user)
        {
            var entity = mapper.Map<Entities.User>(user);

            return context.Users
                .UpdateAsync(
                    u => u.Id == entity.Id,
                    u => new Entities.User
                    {
                        Login = entity.Login,
                            PasswordHash = entity.PasswordHash,
                            FirstName = entity.FirstName,
                            LastName = entity.LastName,
                            Email = entity.Email,
                    }
                );
        }

        public Task DeleteByIdAsync(Guid id)
        {
            return context.Users.DeleteAsync(u => u.Id == id);
        }
    }
}