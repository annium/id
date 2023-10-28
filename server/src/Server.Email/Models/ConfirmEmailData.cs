using System;

namespace Server.Email.Models;

public sealed record ConfirmEmailData(string Server, Guid Id, string Login);
