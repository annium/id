using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinqToDB;

namespace Annium.Id.Db
{
    internal class OrganizationRepository : IOrganizationRepository
    {
        private readonly Entities.IContext context;

        private readonly IMapper mapper;

        public OrganizationRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Organization> CreateAsync(Organization organization)
        {
            var entity = mapper.Map<Entities.Organization>(organization);
            entity.Id = Guid.NewGuid();

            using(var db = context.GetDataConnection())
            {
                await db.InsertAsync(entity);
            }

            return mapper.Map<Organization>(entity);
        }

        public async Task<Organization[]> GetAllAsync()
        {
            var organizations = await context.Organizations.ToArrayAsync();

            return organizations.Select(mapper.Map<Organization>).ToArray();
        }

        public async Task<Organization> GetByIdAsync(Guid id)
        {
            var organization = await context.Organizations
                .FirstOrDefaultAsync(u => u.Id == id);

            return mapper.Map<Organization>(organization);
        }

        public async Task<Organization> FindByKeyAsync(string key)
        {
            var organization = await context.Organizations
                .FirstOrDefaultAsync(u => u.Key == key);

            return mapper.Map<Organization>(organization);
        }

        public async Task<Organization> UpdateAsync(Organization organization)
        {
            var entity = mapper.Map<Entities.Organization>(organization);

            await context.Organizations
                .UpdateAsync(
                    u => u.Id == entity.Id,
                    u => new Entities.Organization
                    {
                        Key = entity.Key,
                            Name = entity.Name,
                            OwnerId = entity.OwnerId,
                    }
                );

            return mapper.Map<Organization>(entity);
        }

        public Task DeleteByIdAsync(Guid id)
        {
            return context.Organizations.DeleteAsync(u => u.Id == id);
        }
    }
}