using System;
using System.Collections.Generic;
using System.Text;
using Micky5991.Samp.Net.Generators.Data;
using Micky5991.Samp.Net.Generators.Extensions;
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

        public override void BuildFunctionBody(IdlFunction function, StringBuilder bodyBuilder, int indent)
        {
            var expectsResult = function.ReturnType != "void";

            bodyBuilder.AppendLine("var arguments = typeConverter.BuildNativeArgumentPointer(new object[] {".Indent(indent));

            foreach (var parameter in function.Parameters)
            {
                bodyBuilder.AppendLine($"{this.parameterBuildStrategy.BuildParameterName(parameter.Name)},".Indent(indent + 1));
            }

            bodyBuilder.AppendLine("});".Indent(indent));
            bodyBuilder.AppendLine();

            bodyBuilder.AppendLine($"var nativeResult = Native.InvokeNative(\"{function.Name}\", \"{BuildNativeInvokeFormat(function.Parameters)}\", arguments);".Indent(indent));

            if (expectsResult == false)
            {
                return;
            }

            bodyBuilder.AppendLine($"var result = ({function.ReturnType}) Convert.ChangeType(nativeResult, typeof({function.ReturnType}));".Indent(indent));

            bodyBuilder.AppendLine();
            bodyBuilder.Append("return result;".Indent(indent));
        }

        public string BuildNativeInvokeFormat(IList<IdlFunctionParameter> parameters)
        {
            var formatBuilder = new StringBuilder();

            for (int i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];

                switch (parameter.Type)
                {
                    case "int":
                        formatBuilder.Append("i");

                        break;
                    case "float":
                        formatBuilder.Append(parameter.Attribute.IsOut() == false ? "f" : "R");

                        break;
                    case "bool":
                        formatBuilder.Append("b");

                        break;

                    default:
                        formatBuilder.Append("r");

                        break;
                }
            }

            return formatBuilder.ToString();
        }
    }
}
