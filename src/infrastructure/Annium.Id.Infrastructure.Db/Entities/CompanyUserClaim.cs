using System;

namespace Annium.Id.Infrastructure.Db.Entities;

internal class CompanyUserClaim : BaseEntity
{
    public Guid CompanyId { get; set; }
    public Guid UserId { get; set; }
    public Guid ClaimId { get; set; }
    public CompanyClaim Claim { get; set; } = null!;
    public string Value { get; set; } = string.Empty;
}