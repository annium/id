using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class CompanyUserRepository : ICompanyUserRepository
    {
        private readonly IContext context;
        private readonly IMapper mapper;

        public CompanyUserRepository(
            IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CompanyUser> SaveAsync(CompanyUser companyUser)
        {
            var entity = await context.CompanyUsers
                .FirstOrDefaultAsync(x => x.CompanyId == companyUser.CompanyId && x.UserId == companyUser.UserId);

            if (entity is null)
            {
                entity = mapper.Map<Entities.CompanyUser>(companyUser);
                context.CompanyUsers.Add(entity);

                await context.SaveChangesAsync();
            }

            return mapper.Map<CompanyUser>(entity);
        }

        public async Task<CompanyUser> GetByIdAsync(Guid companyId, Guid userId)
        {
            var entity = await context.CompanyUsers.AsNoTracking()
                .FirstOrDefaultAsync(x => x.CompanyId == companyId && x.UserId == userId);

            return mapper.Map<CompanyUser>(entity);
        }

        public async Task<User[]> GetAllAsync(Guid companyId)
        {
            var users = await context.CompanyUsers.AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.CompanyId == companyId)
                .Select(x => x.User)
                .ToListAsync();

            return users.Select(mapper.Map<User>).ToArray();
        }

        public async Task DeleteByIdAsync(Guid companyId, Guid userId)
        {
            var entity = await context.CompanyUsers
                .FirstOrDefaultAsync(x => x.CompanyId == companyId && x.UserId == userId);

            if (entity is null)
                return;

            context.CompanyUsers.Remove(entity);

            await context.SaveChangesAsync();
        }
    }
}