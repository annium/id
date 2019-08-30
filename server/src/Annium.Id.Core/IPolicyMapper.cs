using System;
using System.Collections.Generic;

namespace Annium.Id.Core
{
    public interface IPolicyMapper
    {
        void EnsureMappable(
            Policy policy,
            string endpoint,
            IReadOnlyDictionary<string, Type> parameters
        );

        Func<IdAppToken, IReadOnlyDictionary<string, object>, object[]> CreateMapper(Policy policy);
    }
}