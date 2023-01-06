using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Infrastructure.Db.Repositories.Implementations;

internal class CompanyClaimRepository : ICompanyClaimRepository
{
    private readonly IContext _context;
    private readonly IMapper _mapper;

    public CompanyClaimRepository(
        IContext context,
        IMapper mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompanyClaim> CreateAsync(CompanyClaim claim)
    {
        var entity = _mapper.Map<Entities.CompanyClaim>(claim);

        _context.CompanyClaims.Add(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<CompanyClaim>(entity);
    }

    public async Task<CompanyClaim[]> GetAllAsync(Guid appId)
    {
        var claims = await _context.CompanyClaims.AsNoTracking()
            .Where(x => x.AppId == appId)
            .ToArrayAsync();

        return claims.Select(_mapper.Map<CompanyClaim>).ToArray();
    }

    public async Task<CompanyClaim?> GetByIdAsync(Guid id)
    {
        var claim = await _context.CompanyClaims.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<CompanyClaim>(claim);
    }

    public async Task<CompanyClaim?> FindByKeyAsync(Guid appId, string key)
    {
        var claim = await _context.CompanyClaims.AsNoTracking()
            .FirstOrDefaultAsync(x => x.AppId == appId && x.Key == key);

        return _mapper.Map<CompanyClaim>(claim);
    }

    public async Task<CompanyClaim> UpdateAsync(CompanyClaim claim)
    {
        var entity = await _context.CompanyClaims
            .SingleAsync(x => x.Id == claim.Id);

        entity.Key = claim.Key;
        entity.Name = claim.Name;

        await _context.SaveChangesAsync();

        return _mapper.Map<CompanyClaim>(entity);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = await _context.CompanyClaims
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            return;

        _context.CompanyClaims.Remove(entity);

        await _context.SaveChangesAsync();
    }
}