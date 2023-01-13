using Core.Domain.Entities;

namespace Server.Email.Models;

public class RestoreAccessData
{
    public string Server { get; set; } = string.Empty;
    public Tokens Tokens { get; set; } = null!;
}