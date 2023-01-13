using System;
using System.Collections.Generic;

namespace Annium.Id.Core;

public class Policy
{
    public string Name { get; }
    public IReadOnlyDictionary<string, Type> Parameters { get; }
    public Delegate Handle { get; }

    public Policy(
        string name,
        IReadOnlyDictionary<string, Type> parameters,
        Delegate handle
    )
    {
        Name = name;
        Parameters = parameters;
        Handle = handle;
    }
}