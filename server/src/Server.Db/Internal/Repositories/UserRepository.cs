using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.linq2db.Extensions;
using LinqToDB;
using Server.Db.Repositories;
using Server.Domain.Models;

namespace Server.Db.Internal.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly ServerConnection _db;

    public UserRepository(ServerConnection db)
    {
        _db = db;
    }

    public async Task CreateAsync(User user)
    {
        await _db.Users.InsertAsync(user);
    }

    public async Task<User?> TryGetByIdAsync(Guid id)
    {
        var entity = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

        return entity;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var entity = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            throw new InvalidOperationException($"User {id} not found");

        return entity;
    }

    public async Task<IReadOnlyCollection<User>> FindAllByQueryAsync(string query, int limit)
    {
        var entities = await _db.Users.Where(x => x.Login.StartsWith(query)).Take(limit).ToArrayAsync();

        return entities;
    }

    public async Task<User?> TryFindByLoginAsync(string login)
    {
        var entity = await _db.Users.FirstOrDefaultAsync(x => x.Login == login);

        return entity;
    }

    public async Task<User> FindByLoginAsync(string login)
    {
        var entity = await _db.Users.FirstOrDefaultAsync(x => x.Login == login);

        if (entity is null)
            throw new InvalidOperationException($"User with login {login} not found");

        return entity;
    }

    public async Task<User?> TryFindByEmailAsync(string email)
    {
        var entity = await _db.Users.FirstOrDefaultAsync(x => x.Email == email);

        return entity;
    }

    public async Task<User> FindByEmailAsync(string email)
    {
        var entity = await _db.Users.FirstOrDefaultAsync(x => x.Email == email);

        if (entity is null)
            throw new InvalidOperationException($"User with email {email} not found");

        return entity;
    }

    public async Task UpdateAsync(User user)
    {
        await _db.Users.UpdateAsync(user);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await _db.Users.DeleteAsync(x => x.Id == id);
    }
}
