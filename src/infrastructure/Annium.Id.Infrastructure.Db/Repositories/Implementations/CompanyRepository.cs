using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Core.Mapper;
using Annium.Id.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Infrastructure.Db.Repositories.Implementations
{
    internal class CompanyRepository : ICompanyRepository
    {
        private readonly IContext _context;
        private readonly IMapper _mapper;

        public CompanyRepository(
            IContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Company> CreateAsync(Company company)
        {
            var entity = _mapper.Map<Entities.Company>(company);

            _context.Companies.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<Company>(entity);
        }

        public async Task<Company[]> FindAllAsync(string name)
        {
            var query = _context.Companies.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name.StartsWith(name));

            var companies = await query.ToListAsync();

            return companies.Select(_mapper.Map<Company>).ToArray();
        }

        public async Task<Company[]> FindMyAsync(Guid ownerId)
        {
            var companies = await _context.Companies.AsNoTracking()
                .Where(x => x.OwnerId == ownerId)
                .ToListAsync();

            return companies.Select(_mapper.Map<Company>).ToArray();
        }

        public async Task<Company[]> GetAllByIdsAsync(Guid[] ids)
        {
            var companies = await _context.Companies.AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToArrayAsync();

            return companies.Select(_mapper.Map<Company>).ToArray();
        }

        public async Task<Company> GetByIdAsync(Guid id)
        {
            var company = await _context.Companies.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<Company>(company);
        }

        public async Task<Company> UpdateAsync(Company company)
        {
            var entity = await _context.Companies
                .SingleAsync(x => x.Id == company.Id);

            entity.OwnerId = company.OwnerId;
            entity.Name = company.Name;

            await _context.SaveChangesAsync();

            return _mapper.Map<Company>(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await _context.Companies
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
                return;

            _context.Companies.Remove(entity);

            await _context.SaveChangesAsync();
        }
    }
}