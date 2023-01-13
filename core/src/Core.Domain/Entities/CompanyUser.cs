using System;

namespace Core.Domain.Entities;

public class CompanyUser
{
    public Guid CompanyId { get; }
    public Guid UserId { get; }

    public CompanyUser(
        Guid companyId,
        Guid userId
    )
    {
        CompanyId = companyId;
        UserId = userId;
    }
}