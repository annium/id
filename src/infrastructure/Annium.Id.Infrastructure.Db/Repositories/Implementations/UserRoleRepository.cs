using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Infrastructure.Db.Repositories.Implementations
{
    internal class UserRoleRepository : IUserRoleRepository
    {
        private readonly IContext _context;
        private readonly IMapper _mapper;

        public UserRoleRepository(
            IContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserRole> SaveAsync(UserRole userRole)
        {
            var entity = await _context.UserRoles
                .FirstOrDefaultAsync(x => x.UserId == userRole.UserId && x.RoleId == userRole.RoleId);

            if (entity is null)
            {
                entity = _mapper.Map<Entities.UserRole>(userRole);
                _context.UserRoles.Add(entity);
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<UserRole>(entity);
        }

        public async Task<Role[]> GetUserRolesAsync(Guid appId, Guid userId)
        {
            var raw = await _context.UserRoles.AsNoTracking()
                .Include(x => x.Role).ThenInclude(x => x.Claims).ThenInclude(x => x.Claim)
                .Where(x => x.Role.AppId == appId && x.UserId == userId)
                .ToListAsync();

            return raw.Select(x => new Role(
                x.Role.AppId,
                x.Role.Key,
                x.Role.Name,
                x.Role.Claims.Select(y => new ClaimValue(y.ClaimId, y.Claim.Key, y.Claim.Name, y.Value)).ToArray()
            )).ToArray();
        }

        public async Task DeleteByIdAsync(Guid userId, Guid roleId)
        {
            var entity = await _context.UserRoles
                .FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId);

            if (entity is null)
                return;

            _context.UserRoles.Remove(entity);

            await _context.SaveChangesAsync();
        }
    }
}