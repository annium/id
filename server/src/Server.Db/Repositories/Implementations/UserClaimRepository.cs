using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Server.Db.Repositories.Implementations;

internal class UserClaimRepository : IUserClaimRepository
{
    private readonly IContext _context;
    private readonly IMapper _mapper;

    public UserClaimRepository(
        IContext context,
        IMapper mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserClaim> SaveAsync(UserClaim claim)
    {
        var entity = await _context.UserClaims
            .FirstOrDefaultAsync(x => x.UserId == claim.UserId && x.ClaimId == claim.ClaimId);

        if (entity is null)
        {
            entity = _mapper.Map<Entities.UserClaim>(claim);
            _context.UserClaims.Add(entity);
        }
        else
        {
            entity.Value = claim.Value;
        }

        await _context.SaveChangesAsync();

        return _mapper.Map<UserClaim>(entity);
    }

    public async Task<ClaimValue[]> GetUserClaimsAsync(Guid appId, Guid userId)
    {
        var raw = await _context.UserClaims.AsNoTracking()
            .Include(x => x.Claim)
            .Where(x => x.Claim.AppId == appId && x.UserId == userId)
            .ToListAsync();

        return raw.Select(_mapper.Map<ClaimValue>).ToArray();
    }

    public async Task DeleteByIdAsync(Guid userId, Guid claimId)
    {
        var entity = await _context.UserClaims
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ClaimId == claimId);

        if (entity is null)
            return;

        _context.UserClaims.Remove(entity);

        await _context.SaveChangesAsync();
    }
}