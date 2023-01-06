using System;
using Annium.Architecture.CQRS.Commands;

namespace Annium.Id.Api.Domain.Commands.Me;

public class UnregisterMeCommand : ICommand
{
    public Guid MyId { get; private set; }
}