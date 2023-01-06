using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Annium.Id.Infrastructure.Db.Repositories.Implementations;

internal class UserLoginRepository : IUserLoginRepository
{
    private readonly IContext _context;
    private readonly IMapper _mapper;

    public UserLoginRepository(
        IContext context,
        IMapper mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserLogin> CreateAsync(UserLogin login)
    {
        var entity = _mapper.Map<Entities.UserLogin>(login);

        _context.UserLogins.Add(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserLogin>(entity);
    }

    public async Task<UserLogin?> GetByIdAsync(Guid id)
    {
        var entity = await _context.UserLogins.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<UserLogin>(entity);
    }

    public async Task<UserLogin?> FindByRefreshTokenAsync(Guid token)
    {
        var entity = await _context.UserLogins.AsNoTracking()
            .FirstOrDefaultAsync(x => x.RefreshToken == token);

        return _mapper.Map<UserLogin>(entity);
    }

    public async Task<UserLogin> UpdateRefreshTokenAsync(UserLogin login)
    {
        var entity = await _context.UserLogins
            .SingleAsync(x => x.Id == login.Id);

        entity.RefreshToken = login.RefreshToken;
        entity.RefreshTokenExpires = _mapper.Map<DateTime>(login.RefreshTokenExpires);

        await _context.SaveChangesAsync();

        return _mapper.Map<UserLogin>(entity);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = await _context.UserLogins
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            return;

        _context.UserLogins.Remove(entity);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteExpiredByUserIdAsync(Guid userId, Instant instant)
    {
        var expires = _mapper.Map<DateTime>(instant);

        var entities = await _context.UserLogins
            .Where(x => x.UserId == userId && x.RefreshTokenExpires <= expires)
            .ToListAsync();

        if (entities.Count == 0)
            return;

        _context.UserLogins.RemoveRange(entities);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllByUserIdAsync(Guid userId)
    {
        var entities = await _context.UserLogins
            .Where(x => x.UserId == userId)
            .ToListAsync();

        if (entities.Count == 0)
            return;

        _context.UserLogins.RemoveRange(entities);

        await _context.SaveChangesAsync();
    }
}