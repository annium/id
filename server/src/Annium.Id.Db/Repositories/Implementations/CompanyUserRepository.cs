using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using LinqToDB;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class CompanyUserRepository : ICompanyUserRepository
    {
        private readonly Entities.IContext context;
        private readonly IMapper mapper;

        public CompanyUserRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CompanyUser> SaveAsync(CompanyUser companyUser)
        {
            var entity = mapper.Map<Entities.CompanyUser>(companyUser);

            if ((await context.CompanyUsers
                    .CountAsync(ur => ur.CompanyId == entity.CompanyId && ur.UserId == entity.UserId)) == 0)
                using(var db = context.GetDataConnection())
                {
                    await db.InsertAsync(entity);
                }

            return mapper.Map<CompanyUser>(entity);
        }

        public async Task<CompanyUser> GetByIdAsync(Guid companyId, Guid userId)
        {
            var entity = await context.CompanyUsers
                .FirstOrDefaultAsync(cu => cu.CompanyId == companyId && cu.UserId == userId);

            return mapper.Map<CompanyUser>(entity);
        }

        public async Task<User[]> GetAllAsync(Guid companyId)
        {
            var users = await context.CompanyUsers
                .Where(cu => cu.CompanyId == companyId)
                .InnerJoin(context.Users, (cu, u) => cu.UserId == u.Id, (cu, u) => u)
                .ToArrayAsync();

            return users.Select(mapper.Map<User>).ToArray();
        }

        public Task DeleteByIdAsync(Guid companyId, Guid userId)
        {
            return context.CompanyUsers.DeleteAsync(cu => cu.CompanyId == companyId && cu.UserId == userId);
        }
    }
}