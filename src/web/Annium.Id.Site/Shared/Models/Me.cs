using System;

namespace Annium.Id.Site.Shared.Models;

public class Me
{
    public Guid Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}