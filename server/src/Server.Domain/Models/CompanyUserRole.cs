using System;

namespace Server.Domain.Models;

public class CompanyUserRole
{
    public Guid CompanyId { get; private init; }
    public Company Company { get; private init; } = default!;
    public Guid UserId { get; private init; }
    public User User { get; private init; } = default!;
    public Guid RoleId { get; private init; }
    public CompanyRole Role { get; private init; } = default!;

    public CompanyUserRole(Company company, User user, CompanyRole role)
    {
        CompanyId = company.Id;
        Company = company;
        UserId = user.Id;
        User = user;
        RoleId = role.Id;
        Role = role;
    }

    internal CompanyUserRole() { }
}
