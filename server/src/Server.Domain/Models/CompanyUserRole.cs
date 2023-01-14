using System;

namespace Server.Domain.Models;

public class CompanyUserRole
{
    public Guid CompanyId { get; }
    public Guid UserId { get; }
    public Guid RoleId { get; }

    public CompanyUserRole(
        Guid companyId,
        Guid userId,
        Guid roleId
    )
    {
        CompanyId = companyId;
        UserId = userId;
        RoleId = roleId;
    }
}