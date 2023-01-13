using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Server.Db.Repositories.Implementations;

internal class RoleRepository : IRoleRepository
{
    private readonly IContext _context;
    private readonly IMapper _mapper;

    public RoleRepository(
        IContext context,
        IMapper mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Role> CreateAsync(Role role)
    {
        var entity = _mapper.Map<Entities.Role>(role);

        _context.Roles.Add(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<Role>(entity);
    }

    public async Task<Role[]> GetAllAsync(Guid appId)
    {
        var raw = await _context.Roles.AsNoTracking()
            .Include(x => x.Claims).ThenInclude(x => x.Claim)
            .ToListAsync();

        return raw.Select(_mapper.Map<Role>).ToArray();
    }

    public async Task<Role?> TryGetByIdAsync(Guid id)
    {
        var role = await _context.Roles.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return _mapper.Map<Role?>(role);
    }

    public async Task<Role> GetByIdAsync(Guid id)
    {
        var role = await _context.Roles.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (role is null)
            throw new InvalidOperationException($"Role {id} not found");

        return _mapper.Map<Role>(role);
    }

    public async Task<Role?> TryFindByKeyAsync(Guid appId, string key)
    {
        var role = await _context.Roles.AsNoTracking()
            .FirstOrDefaultAsync(c => c.AppId == appId && c.Key == key);

        return _mapper.Map<Role?>(role);
    }

    public async Task<Role> UpdateAsync(Role role)
    {
        var entity = await _context.Roles
            .Include(x => x.Claims).ThenInclude(x => x.Claim)
            .SingleAsync(x => x.Id == role.Id);

        entity.Key = role.Key;
        entity.Name = role.Name;

        await _context.SaveChangesAsync();

        return _mapper.Map<Role>(entity);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = await _context.Roles
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            return;

        _context.Roles.Remove(entity);

        await _context.SaveChangesAsync();
    }
}