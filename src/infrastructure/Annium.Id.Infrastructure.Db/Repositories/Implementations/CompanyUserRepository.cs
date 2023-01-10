using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Infrastructure.Db.Repositories.Implementations;

internal class CompanyUserRepository : ICompanyUserRepository
{
    private readonly IContext _context;
    private readonly IMapper _mapper;

    public CompanyUserRepository(
        IContext context,
        IMapper mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompanyUser> SaveAsync(CompanyUser companyUser)
    {
        var entity = await _context.CompanyUsers
            .FirstOrDefaultAsync(x => x.CompanyId == companyUser.CompanyId && x.UserId == companyUser.UserId);

        if (entity is null)
        {
            entity = _mapper.Map<Entities.CompanyUser>(companyUser);
            _context.CompanyUsers.Add(entity);

            await _context.SaveChangesAsync();
        }

        return _mapper.Map<CompanyUser>(entity);
    }

    public async Task<CompanyUser?> TryGetByIdAsync(Guid companyId, Guid userId)
    {
        var entity = await _context.CompanyUsers.AsNoTracking()
            .FirstOrDefaultAsync(x => x.CompanyId == companyId && x.UserId == userId);

        return _mapper.Map<CompanyUser?>(entity);
    }

    public async Task<User[]> GetAllAsync(Guid companyId)
    {
        var users = await _context.CompanyUsers.AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.CompanyId == companyId)
            .Select(x => x.User)
            .ToListAsync();

        return users.Select(_mapper.Map<User>).ToArray();
    }

    public async Task DeleteByIdAsync(Guid companyId, Guid userId)
    {
        var entity = await _context.CompanyUsers
            .FirstOrDefaultAsync(x => x.CompanyId == companyId && x.UserId == userId);

        if (entity is null)
            return;

        _context.CompanyUsers.Remove(entity);

        await _context.SaveChangesAsync();
    }
}