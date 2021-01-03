using System.Collections.Generic;
using System.Text;
using Micky5991.Samp.Net.Generators.Data;
using Micky5991.Samp.Net.Generators.Extensions;
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
            return function.Attribute.TryGetValue("callback", out _) && function.Attribute.TryGetValue("noimpl", out _) == false;
        }

        public override void BuildFunction(IdlFunction function, BuilderTargetCollection builderTargets, int indent)
        {
            var eventList = builderTargets[BuilderTarget.Events];

            eventList.AppendLine($"this.nativeEventRegistry.RegisterEvent(\"{function.Name}\", \"{this.BuildNativeInvokeFormat(function.Parameters)}\");".Indent(indent + 1));
        }

        private string BuildNativeInvokeFormat(IList<IdlFunctionParameter> parameters)
        {
            var format = new StringBuilder();

            foreach (var parameter in parameters)
            {
                format.Append(this.parameterBuildStrategy.BuildParameterType(parameter.Type) switch
                {
                    "string" => 's',
                    "int" => 'i',
                    "float" => 'f',
                    _ => 'r',
                });
            }

            return format.ToString();
        }
    }
}
