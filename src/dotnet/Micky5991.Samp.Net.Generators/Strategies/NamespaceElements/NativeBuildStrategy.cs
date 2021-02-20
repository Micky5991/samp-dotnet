using System;
using System.Collections.Generic;
using System.Linq;
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
            return function.Attribute.TryGetValue("native", out _) && function.Attribute.TryGetValue("noimpl", out _) == false;
        }

        public override void BuildFunctionBody(IdlFunction function, StringBuilder bodyBuilder, int indent)
        {
            var expectsResult = function.ReturnType != "void";

            bodyBuilder.AppendLine("this.sampThreadEnforcer.EnforceMainThread();".Indent(indent));
            bodyBuilder.AppendLine();

            bodyBuilder.AppendLine("var (arguments, elements) = typeConverter.WriteNativeArguments(new (object Value, int Size)[] {".Indent(indent));

            for (var i = 0; i < function.Parameters.Count; i++)
            {
                var parameter = function.Parameters[i];

                var name = this.parameterBuildStrategy.BuildParameterName(parameter.Name);
                var type = this.parameterBuildStrategy.BuildParameterType(parameter.Type);

                var size = $"sizeof({type})";

                if (type == "string")
                {
                    var followingParameter = function.Parameters.ElementAtOrDefault(i + 1);
                    if (parameter.Attribute.IsOut() &&
                        followingParameter != null &&
                        this.parameterBuildStrategy.BuildParameterType(followingParameter.Type) == "int")
                    {
                        var followingParameterName = this.parameterBuildStrategy.BuildParameterName(followingParameter.Name);

                        size = $"{followingParameterName}";
                    }
                    else
                    {
                        size = $"{name}.Length + 1";
                    }
                }


                bodyBuilder.AppendLine($"({name}, {size}),".Indent(indent + 1));
            }

            bodyBuilder.AppendLine("});".Indent(indent));
            bodyBuilder.AppendLine();

            bodyBuilder.AppendLine($"var nativeResult = Native.InvokeNative(\"{function.Name}\", \"{BuildNativeInvokeFormat(function.Parameters)}\", arguments);".Indent(indent));
            bodyBuilder.AppendLine();

            bodyBuilder.AppendLine("var data = typeConverter.ReadNativeArguments(elements);".Indent(indent));
            bodyBuilder.AppendLine();

            for (var i = 0; i < function.Parameters.Count; i++)
            {
                var parameter = function.Parameters[i];
                if (parameter.Attribute.IsOut() == false)
                {
                    continue;
                }

                var name = this.parameterBuildStrategy.BuildParameterName(parameter.Name);
                var type = this.parameterBuildStrategy.BuildParameterType(parameter.Type);

                bodyBuilder.AppendLine($"{name} = ({type}) data[{i}];".Indent(indent));
            }

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

            for (var i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];

                switch (parameter.Type)
                {
                    case "string":
                        formatBuilder.Append(parameter.Attribute.IsOut() == false ? "s" : $"S[*{i + 1}]");

                        break;
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
