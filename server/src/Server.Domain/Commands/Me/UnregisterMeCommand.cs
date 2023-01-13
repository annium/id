using System;
using Annium.Architecture.CQRS.Commands;

namespace Server.Domain.Commands.Me;

public class UnregisterMeCommand : ICommand
{
    public Guid MyId { get; private set; }
}