using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Microsoft.EntityFrameworkCore;
using Server.Domain.Models;

namespace Server.Db.Repositories.Implementations;

internal class CompanyUserRoleRepository : ICompanyUserRoleRepository
{
    private readonly IContext _context;
    private readonly IMapper _mapper;

    public CompanyUserRoleRepository(
        IContext context,
        IMapper mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompanyUserRole> SaveAsync(CompanyUserRole userRole)
    {
        var entity = await _context.CompanyUserRoles
            .FirstOrDefaultAsync(x => x.CompanyId == userRole.CompanyId && x.UserId == userRole.UserId && x.RoleId == userRole.RoleId);

        if (entity is null)
        {
            entity = _mapper.Map<Entities.CompanyUserRole>(userRole);
            _context.CompanyUserRoles.Add(entity);
            await _context.SaveChangesAsync();
        }

        return _mapper.Map<CompanyUserRole>(entity);
    }

    public async Task<IReadOnlyDictionary<Guid, CompanyRole[]>> GetCompaniesUserRolesAsync(Guid appId, Guid userId)
    {
        var raw = await _context.CompanyUserRoles.AsNoTracking()
            .Include(x => x.Role).ThenInclude(x => x.Claims).ThenInclude(x => x.Claim)
            .Where(x => x.Role.AppId == appId && x.UserId == userId)
            .ToListAsync();

        return raw.GroupBy(x => x.CompanyId)
            .ToDictionary(
                x => x.Key,
                x => x.Select(_mapper.Map<CompanyRole>).ToArray()
            );
    }

    public async Task DeleteByIdAsync(Guid companyId, Guid userId, Guid roleId)
    {
        var entity = await _context.CompanyUserRoles
            .FirstOrDefaultAsync(x => x.CompanyId == companyId && x.UserId == userId && x.RoleId == roleId);

        if (entity is null)
            return;

        _context.CompanyUserRoles.Remove(entity);

        await _context.SaveChangesAsync();
    }
}