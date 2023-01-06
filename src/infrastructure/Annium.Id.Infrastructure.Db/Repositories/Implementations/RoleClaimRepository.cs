using System;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Infrastructure.Db.Repositories.Implementations;

internal class RoleClaimRepository : IRoleClaimRepository
{
    private readonly IContext _context;
    private readonly IMapper _mapper;

    public RoleClaimRepository(
        IContext context,
        IMapper mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RoleClaim> SaveAsync(RoleClaim claim)
    {
        var entity = await _context.RoleClaims
            .FirstOrDefaultAsync(x => x.RoleId == claim.RoleId && x.ClaimId == claim.ClaimId);

        if (entity is null)
        {
            entity = _mapper.Map<Entities.RoleClaim>(claim);
            _context.RoleClaims.Add(entity);
        }
        else
        {
            entity.Value = claim.Value;
        }

        await _context.SaveChangesAsync();

        return _mapper.Map<RoleClaim>(entity);
    }

    public async Task DeleteByIdAsync(Guid roleId, Guid claimId)
    {
        var entity = await _context.RoleClaims
            .FirstOrDefaultAsync(x => x.RoleId == roleId && x.ClaimId == claimId);

        if (entity is null)
            return;

        _context.RoleClaims.Remove(entity);

        await _context.SaveChangesAsync();
    }
}