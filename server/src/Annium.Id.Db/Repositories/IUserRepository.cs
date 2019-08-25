using System;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Db.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);

        Task<User> GetByIdAsync(Guid id);

        Task<User> FindByLoginAsync(string login);

        Task<User> FindByEmailAsync(string email);

        Task<User> UpdateAsync(User user);

        Task DeleteByIdAsync(Guid id);
    }
}