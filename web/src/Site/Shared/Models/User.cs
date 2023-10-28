using System;

namespace Site.Shared.Models;

public class User
{
    public Guid Id { get; set; }
    public string Login { get; set; } = string.Empty;
}
