using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Schema;
using Micky5991.Samp.Net.Generators.Data;
using Micky5991.Samp.Net.Generators.Extensions;
using Micky5991.Samp.Net.Generators.Strategies.Parameters;

namespace Micky5991.Samp.Net.Generators.Strategies.NamespaceElements
{
    public class CallbackBuildStrategy : FunctionBuildStrategy
    {
        private readonly Regex eventNameExpression;

        public CallbackBuildStrategy(ParameterBuildStrategy parameterBuildStrategy)
            : base(parameterBuildStrategy)
        {
            this.eventNameExpression =
                new Regex("^(?:On)?([A-Za-z0-9-_]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        protected override bool IsFunctionAccepted(IdlFunction function)
        {
            return function.Attribute.TryGetValue("callback", out _) && function.Attribute.TryGetValue("noimpl", out _) == false;
        }

        public override void BuildFunction(IdlFunction function, BuilderTargetCollection builderTargets, int indent)
        {
            this.BuildEventRegistration(function, builderTargets, indent + 2);
            this.BuildEventClass(function, builderTargets, indent - 1);
        }

        private string BuildEventName(string callbackName)
        {
            var match = this.eventNameExpression.Match(callbackName);
            var name = callbackName;

            if (match.Success && match.Groups[1].Success)
            {
                name = match.Groups[1].Value;
            }

            return $"Native{name}Event";
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
                    "bool" => 'b',
                    "float" => 'f',
                    _ => 'r',
                });
            }

            return format.ToString();
        }

        private string BuildDefaultReturnValue(IdlAttribute attribute)
        {
            if (attribute.TryGetValue("badret", out var badReturnValue) == false)
            {
                return "false";
            }

            return badReturnValue;
        }

        private void BuildEventRegistration(IdlFunction function, BuilderTargetCollection builderTargets, int indent)
        {
            var eventList = builderTargets[BuilderTarget.Events];

            var format = this.BuildNativeInvokeFormat(function.Parameters);
            var name = this.BuildEventName(function.Name);
            var defaultReturnValue = this.BuildDefaultReturnValue(function.Attribute);

            var parameterNames = new StringBuilder();

            for (var i = 0; i < function.Parameters.Count; i++)
            {
                var parameter = function.Parameters[i];

                parameterNames.Append($"({this.parameterBuildStrategy.BuildParameterType(parameter.Type)}) x[{i}]");

                if (i < function.Parameters.Count - 1)
                {
                    parameterNames.Append(", ");
                }
            }

            eventList.AppendLine($"{{ \"{function.Name}\", \"{format}\", x => new {name}({parameterNames}), {defaultReturnValue} }},".Indent(indent));
        }

        private void BuildEventClass(IdlFunction function, BuilderTargetCollection builderTargets, int indent)
        {
            var functionTargets = new BuilderTargetCollection
            {
                BuilderTarget.Parameters,
                BuilderTarget.EventProperties,
                BuilderTarget.EventBody
            };

            functionTargets.Parent = builderTargets;

            var typesTarget = builderTargets[BuilderTarget.Types];

            var typeName = this.BuildEventName(function.Name);
            var cancellable = function.Attribute.TryGetValue("badret", out _);

            for (var i = 0; i < function.Parameters.Count; i++)
            {
                var parameter = function.Parameters[i];

                this.parameterBuildStrategy.Build(parameter, function, functionTargets, indent + 1);

                if (i < function.Parameters.Count - 1)
                {
                    functionTargets[BuilderTarget.Parameters].Append(", ");
                }
            }

            typesTarget.AppendLine($"public class {typeName} : Micky5991.Samp.Net.Core.NativeEvents.{(cancellable ? "CancellableNativeEvent" : "NativeEvent")}".Indent(indent));
            typesTarget.AppendLine("{".Indent(indent));

            typesTarget.AppendLine(functionTargets[BuilderTarget.EventProperties].ToString());

            typesTarget.AppendLine($"public {typeName}({functionTargets[BuilderTarget.Parameters]})".Indent(indent + 1));
            typesTarget.AppendLine("{".Indent(indent + 1));

            typesTarget.Append(functionTargets[BuilderTarget.EventBody].ToString());

            typesTarget.AppendLine("}".Indent(indent + 1));

            typesTarget.AppendLine("}".Indent(indent));
            typesTarget.AppendLine();
        }
    }
}
