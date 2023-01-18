using Server.Domain.Models;

namespace Server.Email.Models;

public sealed record RestoreAccessData(string Server, string Login, Tokens Tokens);