using System;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Microsoft.EntityFrameworkCore;
using Server.Domain.Models;

namespace Server.Db.Repositories.Implementations;

internal class CompanyRoleClaimRepository : ICompanyRoleClaimRepository
{
    private readonly IContext _context;
    private readonly IMapper _mapper;

    public CompanyRoleClaimRepository(
        IContext context,
        IMapper mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompanyRoleClaim> SaveAsync(CompanyRoleClaim claim)
    {
        var entity = await _context.CompanyRoleClaims
            .FirstOrDefaultAsync(x => x.RoleId == claim.RoleId && x.ClaimId == claim.ClaimId);

        if (entity is null)
        {
            entity = _mapper.Map<Entities.CompanyRoleClaim>(claim);
            _context.CompanyRoleClaims.Add(entity);
        }
        else
        {
            entity.Value = claim.Value;
        }

        await _context.SaveChangesAsync();

        return _mapper.Map<CompanyRoleClaim>(entity);
    }

    public async Task DeleteByIdAsync(Guid roleId, Guid claimId)
    {
        var entity = await _context.CompanyRoleClaims
            .FirstOrDefaultAsync(x => x.RoleId == roleId && x.ClaimId == claimId);

        if (entity is null)
            return;

        _context.CompanyRoleClaims.Remove(entity);

        await _context.SaveChangesAsync();
    }
}