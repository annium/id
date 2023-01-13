using System;

namespace Server.Email.Models;

public class ConfirmEmailData
{
    public string Server { get; set; } = string.Empty;
    public Guid Id { get; set; }
}