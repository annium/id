using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Microsoft.EntityFrameworkCore;
using Server.Domain.Models;

namespace Server.Db.Repositories.Implementations;

internal class CompanyUserClaimRepository : ICompanyUserClaimRepository
{
    private readonly IContext _context;
    private readonly IMapper _mapper;

    public CompanyUserClaimRepository(
        IContext context,
        IMapper mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompanyUserClaim> SaveAsync(CompanyUserClaim claim)
    {
        var entity = await _context.CompanyUserClaims
            .FirstOrDefaultAsync(x => x.CompanyId == claim.CompanyId && x.UserId == claim.UserId && x.ClaimId == claim.ClaimId);

        if (entity is null)
        {
            entity = _mapper.Map<Entities.CompanyUserClaim>(claim);
            _context.CompanyUserClaims.Add(entity);
        }
        else
        {
            entity.Value = claim.Value;
        }

        await _context.SaveChangesAsync();

        return _mapper.Map<CompanyUserClaim>(entity);
    }

    public async Task<IReadOnlyDictionary<Guid, ClaimValue[]>> GetCompaniesUserClaimsAsync(Guid appId, Guid userId)
    {
        var raw = await _context.CompanyUserClaims.AsNoTracking()
            .Include(x => x.Claim)
            .Where(x => x.Claim.AppId == appId && x.UserId == userId)
            .ToListAsync();

        return raw.GroupBy(x => x.CompanyId)
            .ToDictionary(
                x => x.Key,
                x => x.Select(_mapper.Map<ClaimValue>).ToArray()
            );
    }

    public async Task DeleteByIdAsync(Guid companyId, Guid userId, Guid claimId)
    {
        var entity = await _context.CompanyUserClaims
            .FirstOrDefaultAsync(x => x.CompanyId == companyId && x.UserId == userId && x.ClaimId == claimId);

        if (entity is null)
            return;

        _context.CompanyUserClaims.Remove(entity);

        await _context.SaveChangesAsync();
    }
}