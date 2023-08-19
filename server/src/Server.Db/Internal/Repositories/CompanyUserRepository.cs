using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.linq2db.Extensions.Extensions;
using LinqToDB;
using Server.Db.Repositories;
using Server.Domain.Models;

namespace Server.Db.Internal.Repositories;

internal class CompanyUserRepository : RepositoryBase, ICompanyUserRepository
{
    public CompanyUserRepository(ServerConnection db) : base(db)
    {
    }

    public async Task SaveAsync(CompanyUser companyUser)
    {
        await Db.CompanyUsers.InsertOrUpdateAsync(companyUser);
    }

    public async Task<CompanyUser?> TryGetByIdAsync(Guid companyId, Guid userId)
    {
        var entity = await Db.CompanyUsers
            .FirstOrDefaultAsync(x => x.CompanyId == companyId && x.UserId == userId);

        return entity;
    }

    public async Task<IReadOnlyCollection<User>> GetAllAsync(Guid companyId)
    {
        var entities = await Db.CompanyUsers
            .LoadWith(x => x.User)
            .Where(x => x.CompanyId == companyId)
            .Select(x => x.User)
            .ToArrayAsync();

        return entities;
    }

    public async Task DeleteByIdAsync(Guid companyId, Guid userId)
    {
        await Db.CompanyUsers.DeleteAsync(x => x.CompanyId == companyId && x.UserId == userId);
    }
}