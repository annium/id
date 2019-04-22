using System;
using System.Threading.Tasks;
using Annium.Extensions.Mapper;
using LinqToDB;

namespace Annium.Id.Db
{
    internal class CompanyUserRoleRepository : ICompanyUserRoleRepository
    {
        private readonly Entities.IContext context;

        private readonly IMapper mapper;

        public CompanyUserRoleRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CompanyUserRole> SaveAsync(CompanyUserRole userRole)
        {
            var entity = mapper.Map<Entities.CompanyUserRole>(userRole);

            if ((await context.CompanyUserRoles
                    .CountAsync(ur => ur.CompanyId == entity.CompanyId && ur.UserId == entity.UserId && ur.RoleId == entity.RoleId)) == 0)
                using(var db = context.GetDataConnection())
                {
                    await db.InsertAsync(entity);
                }

            return mapper.Map<CompanyUserRole>(entity);
        }

        public Task DeleteByIdAsync(Guid companyId, Guid userId, Guid roleId)
        {
            return context.CompanyUserRoles.DeleteAsync(ur => ur.CompanyId == companyId && ur.UserId == userId && ur.RoleId == roleId);
        }
    }
}