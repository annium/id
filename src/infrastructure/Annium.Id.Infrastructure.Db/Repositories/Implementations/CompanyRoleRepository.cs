using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Infrastructure.Db.Repositories.Implementations
{
    internal class CompanyRoleRepository : ICompanyRoleRepository
    {
        private readonly IContext _context;
        private readonly IMapper _mapper;

        public CompanyRoleRepository(
            IContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CompanyRole> CreateAsync(CompanyRole role)
        {
            var entity = _mapper.Map<Entities.CompanyRole>(role);

            _context.CompanyRoles.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<CompanyRole>(entity);
        }

        public async Task<CompanyRole[]> GetAllAsync(Guid appId)
        {
            var raw = await _context.CompanyRoles.AsNoTracking()
                .Include(x => x.Claims).ThenInclude(x => x.Claim)
                .ToListAsync();

            return raw.Select(_mapper.Map<CompanyRole>).ToArray();
        }

        public async Task<CompanyRole> GetByIdAsync(Guid id)
        {
            var role = await _context.CompanyRoles.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return _mapper.Map<CompanyRole>(role);
        }

        public async Task<CompanyRole> FindByKeyAsync(Guid appId, string key)
        {
            var role = await _context.CompanyRoles.AsNoTracking()
                .FirstOrDefaultAsync(c => c.AppId == appId && c.Key == key);

            return _mapper.Map<CompanyRole>(role);
        }

        public async Task<CompanyRole> UpdateAsync(CompanyRole role)
        {
            var entity = await _context.CompanyRoles
                .Include(x => x.Claims).ThenInclude(x => x.Claim)
                .SingleAsync(x => x.Id == role.Id);

            entity.Key = role.Key;
            entity.Name = role.Name;

            await _context.SaveChangesAsync();

            return _mapper.Map<CompanyRole>(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await _context.CompanyRoles
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
                return;

            _context.CompanyRoles.Remove(entity);

            await _context.SaveChangesAsync();
        }
    }
}