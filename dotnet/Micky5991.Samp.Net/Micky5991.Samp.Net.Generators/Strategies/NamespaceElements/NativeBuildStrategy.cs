using Micky5991.Samp.Net.Generators.Data;
using Micky5991.Samp.Net.Generators.Strategies.Parameters;

namespace Micky5991.Samp.Net.Generators.Strategies.NamespaceElements
{
    public class NativeBuildStrategy : FunctionBuildStrategy
    {
        public NativeBuildStrategy(ParameterBuildStrategy parameterBuildStrategy)
            : base(parameterBuildStrategy)
        {
        }

        protected override bool IsFunctionAccepted(IdlFunction function)
        {
            return function.Attribute.TryGetValue("native", out _);
        }
    }
}
