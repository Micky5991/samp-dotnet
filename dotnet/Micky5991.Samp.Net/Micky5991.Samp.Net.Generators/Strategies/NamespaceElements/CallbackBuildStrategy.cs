using System.Text;
using Micky5991.Samp.Net.Generators.Data;
using Micky5991.Samp.Net.Generators.Strategies.Parameters;

namespace Micky5991.Samp.Net.Generators.Strategies.NamespaceElements
{
    public class CallbackBuildStrategy : FunctionBuildStrategy
    {
        public CallbackBuildStrategy(ParameterBuildStrategy parameterBuildStrategy)
            : base(parameterBuildStrategy)
        {
        }

        protected override bool IsFunctionAccepted(IdlFunction function)
        {
            return false;
            // return function.Attribute.TryGetValue("callback", out _);
        }

        public override void BuildFunction(IdlFunction function, BuilderTargetCollection builderTargets, int indent)
        {

        }
    }
}
