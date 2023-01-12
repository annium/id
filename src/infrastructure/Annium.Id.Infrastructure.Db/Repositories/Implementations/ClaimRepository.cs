using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Infrastructure.Db.Repositories.Implementations;

internal class ClaimRepository : IClaimRepository
{
    private readonly IContext _context;
    private readonly IMapper _mapper;

    public ClaimRepository(
        IContext context,
        IMapper mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Claim> CreateAsync(Claim claim)
    {
        var entity = _mapper.Map<Entities.Claim>(claim);

        _context.Claims.Add(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<Claim>(entity);
    }

    public async Task<Claim[]> GetAllAsync(Guid appId)
    {
        var claims = await _context.Claims.AsNoTracking()
            .Where(x => x.AppId == appId)
            .ToArrayAsync();

        return claims.Select(_mapper.Map<Claim>).ToArray();
    }

    public async Task<Claim?> TryGetByIdAsync(Guid id)
    {
        var claim = await _context.Claims.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<Claim?>(claim);
    }

    public async Task<Claim> GetByIdAsync(Guid id)
    {
        var claim = await _context.Claims.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (claim is null)
            throw new InvalidOperationException($"Claim {id} not found");

        return _mapper.Map<Claim>(claim);
    }

    public async Task<Claim?> TryFindByKeyAsync(Guid appId, string key)
    {
        var claim = await _context.Claims.AsNoTracking()
            .FirstOrDefaultAsync(x => x.AppId == appId && x.Key == key);

        return _mapper.Map<Claim?>(claim);
    }

    public async Task<Claim> UpdateAsync(Claim claim)
    {
        var entity = await _context.Claims
            .SingleAsync(x => x.Id == claim.Id);

        entity.Key = claim.Key;
        entity.Name = claim.Name;

        await _context.SaveChangesAsync();

        return _mapper.Map<Claim>(entity);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = await _context.Claims
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            return;

        _context.Claims.Remove(entity);

        await _context.SaveChangesAsync();
    }
}