using System;

namespace Server.Domain.Models;

public class CompanyUser
{
    public Guid CompanyId { get; private init; }
    public Company Company { get; private init; } = default!;
    public Guid UserId { get; private init; }
    public User User { get; private init; } = default!;

    public CompanyUser(Company company, User user)
    {
        CompanyId = company.Id;
        Company = company;
        UserId = user.Id;
        User = user;
    }

    internal CompanyUser() { }
}
