using System;
using System.Threading.Tasks;
using NodaTime;

namespace Annium.Id.Db
{
    public interface IUserLoginRepository
    {
        Task<UserLogin> CreateAsync(UserLogin login);

        Task<ValueTuple<Guid, Instant>> UpdateRefreshTokenAsync(Guid token, Instant expires);

        Task DeleteByIdAsync(Guid id);

        Task DeleteExpiredByUserIdAsync(Guid userId, Instant instant);

        Task DeleteAllByUserIdAsync(Guid userId);
    }
}