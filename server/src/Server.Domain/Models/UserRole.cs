using System;

namespace Server.Domain.Models;

public class UserRole
{
    public Guid UserId { get; private init; }
    public User User { get; private init; } = default!;
    public Guid RoleId { get; private init; }
    public Role Role { get; private init; } = default!;

    public UserRole(User user, Role role)
    {
        UserId = user.Id;
        User = user;
        RoleId = role.Id;
        Role = role;
    }

    internal UserRole() { }
}
