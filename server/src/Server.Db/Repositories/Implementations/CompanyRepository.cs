using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.linq2db.Extensions.Extensions;
using LinqToDB;
using Server.Domain.Models;

namespace Server.Db.Repositories.Implementations;

internal class CompanyRepository : RepositoryBase, ICompanyRepository
{
    public CompanyRepository(ServerConnection db) : base(db)
    {
    }

    public async Task CreateAsync(Company company)
    {
        await Db.Companies.InsertAsync(company);
    }

    public async Task<IReadOnlyCollection<Company>> FindAllAsync(string name)
    {
        IQueryable<Company> query = Db.Companies;

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(x => x.Name.StartsWith(name));

        var entities = await query.ToArrayAsync();

        return entities;
    }

    public async Task<IReadOnlyCollection<Company>> FindMyAsync(Guid ownerId)
    {
        var entities = await Db.Companies
            .Where(x => x.OwnerId == ownerId)
            .ToArrayAsync();

        return entities;
    }

    public async Task<IReadOnlyCollection<Company>> GetAllByIdsAsync(IReadOnlyCollection<Guid> ids)
    {
        var entities = await Db.Companies
            .Where(x => ids.Contains(x.Id))
            .ToArrayAsync();

        return entities;
    }

    public async Task<Company?> TryGetByIdAsync(Guid id)
    {
        var entity = await Db.Companies
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity;
    }

    public async Task<Company> GetByIdAsync(Guid id)
    {
        var entity = await Db.Companies
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            throw new InvalidOperationException($"Company {id} not found");

        return entity;
    }

    public async Task UpdateAsync(Company company)
    {
        await Db.Companies.UpdateAsync(company);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await Db.Companies.DeleteAsync(x => x.Id == id);
    }
}