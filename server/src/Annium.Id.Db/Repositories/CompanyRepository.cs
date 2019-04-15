using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinqToDB;

namespace Annium.Id.Db
{
    internal class CompanyRepository : ICompanyRepository
    {
        private readonly Entities.IContext context;

        private readonly IMapper mapper;

        public CompanyRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Company> CreateAsync(Company company)
        {
            var entity = mapper.Map<Entities.Company>(company);
            entity.Id = Guid.NewGuid();

            using(var db = context.GetDataConnection())
            {
                await db.InsertAsync(entity);
            }

            return mapper.Map<Company>(entity);
        }

        public async Task<Company[]> GetAllAsync()
        {
            var companies = await context.Companies.ToArrayAsync();

            return companies.Select(mapper.Map<Company>).ToArray();
        }

        public async Task<Company> GetByIdAsync(Guid id)
        {
            var company = await context.Companies
                .FirstOrDefaultAsync(u => u.Id == id);

            return mapper.Map<Company>(company);
        }

        public async Task<Company> FindByKeyAsync(string key)
        {
            var company = await context.Companies
                .FirstOrDefaultAsync(u => u.Key == key);

            return mapper.Map<Company>(company);
        }

        public async Task<Company> UpdateAsync(Company company)
        {
            var entity = mapper.Map<Entities.Company>(company);

            await context.Companies
                .UpdateAsync(
                    u => u.Id == entity.Id,
                    u => new Entities.Company
                    {
                        Key = entity.Key,
                            Name = entity.Name,
                            OwnerId = entity.OwnerId,
                    }
                );

            return mapper.Map<Company>(entity);
        }

        public Task DeleteByIdAsync(Guid id)
        {
            return context.Companies.DeleteAsync(u => u.Id == id);
        }
    }
}