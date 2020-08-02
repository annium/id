using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Infrastructure.Db.Repositories.Implementations
{
    internal class UserRepository : IUserRepository
    {
        private readonly IContext _context;
        private readonly IMapper _mapper;

        public UserRepository(
            IContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<User> CreateAsync(User user)
        {
            var entity = _mapper.Map<Entities.User>(user);

            _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<User>(entity);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var entity = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<User>(entity);
        }

        public async Task<User[]> FindAllByQueryAsync(string query, int limit)
        {
            var entities = await _context.Users.AsNoTracking()
                .Where(x => x.Login.StartsWith(query))
                .Take(limit)
                .ToArrayAsync();

            return _mapper.Map<User[]>(entities);
        }

        public async Task<User> FindByLoginAsync(string login)
        {
            var entity = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == login);

            return _mapper.Map<User>(entity);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var entity = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);

            return _mapper.Map<User>(entity);
        }

        public async Task<User> UpdateAsync(User user)
        {
            var entity = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == user.Id);

            entity.Login = user.Login;
            entity.PasswordHash = user.PasswordHash;
            entity.Email = user.Email;

            await _context.SaveChangesAsync();

            return _mapper.Map<User>(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
                return;

            _context.Users.Remove(entity);

            await _context.SaveChangesAsync();
        }
    }
}