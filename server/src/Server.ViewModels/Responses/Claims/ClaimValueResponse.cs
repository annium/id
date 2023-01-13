using System;

namespace Server.ViewModels.Responses.Claims;

public class ClaimValueResponse
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}