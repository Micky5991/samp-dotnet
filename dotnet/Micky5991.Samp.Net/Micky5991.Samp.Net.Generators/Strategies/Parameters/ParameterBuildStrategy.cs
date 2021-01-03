using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Micky5991.Samp.Net.Generators.Data;
using Micky5991.Samp.Net.Generators.Extensions;

namespace Micky5991.Samp.Net.Generators.Strategies.Parameters
{
    public class ParameterBuildStrategy
    {
        private readonly Regex parametersExpression;

        public ParameterBuildStrategy()
        {
            this.parametersExpression = new Regex(@"(?:\[(?<attribute>[^]]+)\])?\s?(?<type>[A-Za-z0-9\-_ \*]+?)\s+(?<name>[A-Za-z0-9\-_]+)(?:\s*=\s*(?<default>[^\,]+))?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public IList<IdlFunctionParameter> Parse(string parameters)
        {
            var result = new List<IdlFunctionParameter>();

            var matches = this.parametersExpression.Matches(parameters);

            for (var i = 0; i < matches.Count; i++)
            {
                var name = matches[i].Groups["name"].Value;
                var type = matches[i].Groups["type"].Value;
                var attributes = matches[i].Groups["attribute"].Value;
                var defaultValue = matches[i].Groups["default"].Value;

                result.Add(new IdlFunctionParameter(type, name, new IdlAttribute(attributes), defaultValue));
            }

            return result;
        }

        public void Build(IdlFunctionParameter parameter, IdlFunction function, BuilderTargetCollection functionTargets, int indent)
        {
            if (functionTargets.TryGetValue(BuilderTarget.Parameters, out var parameterTarget))
            {
                this.BuildParameterDeclaration(parameter, parameterTarget);
            }

            if (functionTargets.TryGetValue(BuilderTarget.FunctionBody, out var bodyTarget))
            {
                this.BuildBodyInstructions(parameter, bodyTarget, indent);
            }

            if (functionTargets.TryGetValue(BuilderTarget.EventProperties, out var propertyTarget))
            {
                this.BuildEventProperties(parameter, propertyTarget, indent);
            }

            if (functionTargets.TryGetValue(BuilderTarget.EventBody, out var eventBodyTarget))
            {
                this.BuildPropertyAssignment(parameter, eventBodyTarget, indent + 1);
            }
        }

        private void BuildBodyInstructions(IdlFunctionParameter parameter, StringBuilder bodyBuilder, int indent)
        {
            if (parameter.Attribute.IsOut())
            {
                bodyBuilder.AppendLine($"{this.BuildParameterName(parameter.Name)} = default;".Indent(indent));
            }
        }

        private void BuildEventProperties(IdlFunctionParameter parameter, StringBuilder propertyBuilder, int indent)
        {
            propertyBuilder.AppendLine($"public {this.BuildParameterType(parameter.Type)} {this.BuildPropertyName(parameter.Name)} {{ get; }}".Indent(indent));
        }

        private void BuildPropertyAssignment(IdlFunctionParameter parameter, StringBuilder propertyBuilder, int indent)
        {
            propertyBuilder.AppendLine($"this.{this.BuildPropertyName(parameter.Name)} = {this.BuildParameterName(parameter.Name)};".Indent(indent));
        }

        private void BuildParameterDeclaration(IdlFunctionParameter parameter, StringBuilder parametersBuilder)
        {
            var type = this.BuildParameterType(parameter.Type);
            var name = this.BuildParameterName(parameter.Name);

            if (parameter.Attribute.IsInAndOut())
            {
                parametersBuilder.Append("ref ");
            }
            else if (parameter.Attribute.IsIn())
            {
                parametersBuilder.Append("in ");
            }
            else if (parameter.Attribute.IsOut())
            {
                parametersBuilder.Append("out ");
            }

            parametersBuilder.Append($"{type} {name}");
        }

        public string BuildParameterType(string type)
        {
            return type switch
            {
                "TimerCallback" => typeof(Action).FullName,
                "void" => typeof(IntPtr).FullName,
                _ => type
            };
        }

        public string BuildParameterName(string name)
        {
            name = name switch
            {
                "string" => "value",
                "param" => "parameters",
                _ => name,
            };

            return name.ConvertToCamelCase();
        }

        public string BuildPropertyName(string name)
        {
            name = name switch
            {
                "string" => "value",
                "param" => "parameters",
                _ => name,
            };

            return name.ConvertToPascalCase();
        }
    }
}
