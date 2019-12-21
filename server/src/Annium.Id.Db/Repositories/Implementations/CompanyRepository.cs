using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Db.Repositories.Implementations
{
    internal class CompanyRepository : ICompanyRepository
    {
        private readonly IContext context;
        private readonly IMapper mapper;

        public CompanyRepository(
            IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Company> CreateAsync(Company company)
        {
            var entity = mapper.Map<Entities.Company>(company);

            context.Companies.Add(entity);
            await context.SaveChangesAsync();

            return mapper.Map<Company>(entity);
        }

        public async Task<Company[]> GetAllByIdsAsync(Guid[] ids)
        {
            var companies = await context.Companies.AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToArrayAsync();

            return companies.Select(mapper.Map<Company>).ToArray();
        }

        public async Task<Company> GetByIdAsync(Guid id)
        {
            var company = await context.Companies.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<Company>(company);
        }

        public async Task<Company> UpdateAsync(Company company)
        {
            var entity = await context.Companies
                .SingleAsync(x => x.Id == company.Id);

            entity.OwnerId = company.OwnerId;
            entity.Name = company.Name;

            await context.SaveChangesAsync();

            return mapper.Map<Company>(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await context.Companies
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
                return;

            context.Companies.Remove(entity);

            await context.SaveChangesAsync();
        }
    }
}