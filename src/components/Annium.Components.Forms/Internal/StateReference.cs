using System.Reflection;
using Annium.Extensions.Primitives;

namespace Annium.Components.Forms.Internal
{
    internal class StateReference
    {
        public IState Ref { get; }
        public MethodInfo Get { get; }
        public MethodInfo Set { get; }

        public StateReference(
            IState @ref,
            MethodInfo get,
            MethodInfo set
        )
        {
            Ref = @ref;
            Get = get;
            Set = set;
        }

        public override string ToString() => Ref.GetType().FriendlyName();
    }
}