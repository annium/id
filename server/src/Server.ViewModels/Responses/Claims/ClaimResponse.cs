using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Models;

namespace Server.ViewModels.Responses.Claims;

public record ClaimResponse : IResponse<Claim>
{
    public Guid Id { get; set; }
    public Guid AppId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
