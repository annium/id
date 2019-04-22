using System;
using System.Threading.Tasks;
using Annium.Extensions.Mapper;
using LinqToDB;

namespace Annium.Id.Db
{
    internal class UserRoleRepository : IUserRoleRepository
    {
        private readonly Entities.IContext context;

        private readonly IMapper mapper;

        public UserRoleRepository(
            Entities.IContext context,
            IMapper mapper
        )
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<UserRole> SaveAsync(UserRole userRole)
        {
            var entity = mapper.Map<Entities.UserRole>(userRole);

            if ((await context.UserRoles.CountAsync(ur => ur.UserId == entity.UserId && ur.RoleId == entity.RoleId)) == 0)
                using(var db = context.GetDataConnection())
                {
                    await db.InsertAsync(entity);
                }

            return mapper.Map<UserRole>(entity);
        }

        public Task DeleteByIdAsync(Guid userId, Guid roleId)
        {
            return context.UserRoles.DeleteAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        }
    }
}