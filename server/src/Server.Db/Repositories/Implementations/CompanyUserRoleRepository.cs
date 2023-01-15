using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.linq2db.Extensions.Extensions;
using LinqToDB;
using Server.Domain.Models;

namespace Server.Db.Repositories.Implementations;

internal class CompanyUserRoleRepository : ICompanyUserRoleRepository
{
    private readonly ServerConnection _db;

    public CompanyUserRoleRepository(ServerConnection db)
    {
        _db = db;
    }

    public async Task SaveAsync(CompanyUserRole userRole)
    {
        await _db.CompanyUserRoles.InsertOrUpdateAsync(userRole);
    }

    public async Task<IReadOnlyDictionary<Guid, IReadOnlyCollection<CompanyRole>>> GetCompaniesUserRolesAsync(Guid appId, Guid userId)
    {
        var entities = await _db.CompanyUserRoles
            .LoadWith(x => x.Role).ThenLoad(x => x.Claims).ThenLoad(x => x.Claim)
            .Where(x => x.Role.AppId == appId && x.UserId == userId)
            .ToArrayAsync();

        return entities.GroupBy(x => x.CompanyId)
            .ToDictionary(
                x => x.Key,
                x => (IReadOnlyCollection<CompanyRole>) x.Select(y => y.Role).ToArray()
            );
    }

    public async Task DeleteByIdAsync(Guid companyId, Guid userId, Guid roleId)
    {
        await _db.CompanyUserRoles.DeleteAsync(x => x.CompanyId == companyId && x.UserId == userId && x.RoleId == roleId);
    }
}