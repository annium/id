using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.linq2db.Extensions.Extensions;
using LinqToDB;
using Server.Domain.Models;

namespace Server.Db.Repositories.Implementations;

internal class CompanyRepository : ICompanyRepository
{
    private readonly ServerConnection _db;

    public CompanyRepository(ServerConnection db)
    {
        _db = db;
    }

    public async Task CreateAsync(Company company)
    {
        await _db.Companies.InsertAsync(company);
    }

    public async Task<IReadOnlyCollection<Company>> FindAllAsync(string name)
    {
        IQueryable<Company> query = _db.Companies;

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(x => x.Name.StartsWith(name));

        var entities = await query.ToArrayAsync();

        return entities;
    }

    public async Task<IReadOnlyCollection<Company>> FindMyAsync(Guid ownerId)
    {
        var entities = await _db.Companies
            .Where(x => x.OwnerId == ownerId)
            .ToArrayAsync();

        return entities;
    }

    public async Task<IReadOnlyCollection<Company>> GetAllByIdsAsync(IReadOnlyCollection<Guid> ids)
    {
        var entities = await _db.Companies
            .Where(x => ids.Contains(x.Id))
            .ToArrayAsync();

        return entities;
    }

    public async Task<Company?> TryGetByIdAsync(Guid id)
    {
        var entity = await _db.Companies
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity;
    }

    public async Task<Company> GetByIdAsync(Guid id)
    {
        var entity = await _db.Companies
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            throw new InvalidOperationException($"Company {id} not found");

        return entity;
    }

    public async Task UpdateAsync(Company company)
    {
        await _db.Companies.UpdateAsync(company);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await _db.Companies.DeleteAsync(x => x.Id == id);
    }
}