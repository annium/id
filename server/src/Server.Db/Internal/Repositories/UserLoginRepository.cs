using System;
using System.Threading.Tasks;
using Annium.linq2db.Extensions.Extensions;
using LinqToDB;
using NodaTime;
using Server.Db.Repositories;
using Server.Domain.Models;

namespace Server.Db.Internal.Repositories;

internal class UserLoginRepository : IUserLoginRepository
{
    private readonly ServerConnection _db;

    public UserLoginRepository(ServerConnection db)
    {
        _db = db;
    }

    public async Task CreateAsync(UserLogin login)
    {
        await _db.UserLogins.InsertAsync(login);
    }

    public async Task<UserLogin?> TryGetByIdAsync(Guid id)
    {
        var entity = await _db.UserLogins
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity;
    }

    public async Task<UserLogin> GetByIdAsync(Guid id)
    {
        var entity = await _db.UserLogins
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            throw new InvalidOperationException($"Claim {id} not found");

        return entity;
    }

    public async Task<UserLogin?> TryFindByRefreshTokenAsync(Guid token)
    {
        var entity = await _db.UserLogins
            .FirstOrDefaultAsync(x => x.RefreshToken == token);

        return entity;
    }

    public async Task<UserLogin> FindByRefreshTokenAsync(Guid token)
    {
        var entity = await _db.UserLogins
            .FirstOrDefaultAsync(x => x.RefreshToken == token);

        if (entity is null)
            throw new InvalidOperationException($"User login with refresh token {token} not found");

        return entity;
    }

    public async Task UpdateRefreshTokenAsync(UserLogin login)
    {
        await _db.UserLogins.UpdateAsync(login);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await _db.UserLogins.DeleteAsync(x => x.Id == id);
    }

    public async Task DeleteExpiredByUserIdAsync(Guid userId, Instant instant)
    {
        await _db.UserLogins.DeleteAsync(x => x.UserId == userId && x.RefreshTokenExpires <= instant);
    }

    public async Task DeleteAllByUserIdAsync(Guid id)
    {
        await _db.UserLogins.DeleteAsync(x => x.UserId == id);
    }
}