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

        public void Build(IdlFunctionParameter parameter, IdlFunction function, StringBuilder parametersBuilder, StringBuilder bodyBuilder, int indent)
        {
            this.BuildParameterDeclaration(parameter, parametersBuilder);
            this.BuildBodyInstructions(parameter, bodyBuilder, indent);
        }

        private void BuildBodyInstructions(IdlFunctionParameter parameter, StringBuilder bodyBuilder, int indent)
        {
            if (parameter.Attribute.IsOut())
            {
                bodyBuilder.AppendLine($"{this.BuildParameterName(parameter.Name)} = default;".Indent(indent));
            }
        }

        private void BuildParameterDeclaration(IdlFunctionParameter parameter, StringBuilder parametersBuilder)
        {
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

            parametersBuilder.Append($"{this.BuildParameterType(parameter.Type)} {this.BuildParameterName(parameter.Name)}");
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
    }
}
