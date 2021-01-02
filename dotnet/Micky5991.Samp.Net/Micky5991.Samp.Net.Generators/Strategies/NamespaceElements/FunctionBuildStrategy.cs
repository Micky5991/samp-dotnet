using System.Text;
using System.Text.RegularExpressions;
using Micky5991.Samp.Net.Generators.Contracts;
using Micky5991.Samp.Net.Generators.Data;
using Micky5991.Samp.Net.Generators.Extensions;
using Micky5991.Samp.Net.Generators.Strategies.Parameters;

namespace Micky5991.Samp.Net.Generators.Strategies.NamespaceElements
{
    public abstract class FunctionBuildStrategy : IElementBuildStrategy
    {
        protected readonly ParameterBuildStrategy parameterBuildStrategy;

        protected readonly Regex functionExpression;

        public FunctionBuildStrategy(ParameterBuildStrategy parameterBuildStrategy)
        {
            this.parameterBuildStrategy = parameterBuildStrategy;
            this.functionExpression = new Regex(@"^(?:\[(?<attribute>.*?)\]\s+)?(?<return>[A-z0-9_\s]+)\s(?<name>[A-z0-9_]+)\((?<parameters>.*)\);$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        protected abstract bool IsFunctionAccepted(IdlFunction function);

        public bool IsLineAccepted(string line)
        {
            if (this.functionExpression.IsMatch(line) == false)
            {
                return false;
            }

            return this.IsFunctionAccepted((IdlFunction) this.Parse(line));
        }

        public IdlNamespaceElement Parse(string line)
        {
            var match = this.functionExpression.Match(line);

            var name = match.Groups["name"].Value;
            var returnValue = match.Groups["return"].Value;
            var attribute = match.Groups["attribute"].Value;
            var parameters = match.Groups["parameters"].Value;

            return this.ParseFunction(name, returnValue, attribute, parameters);
        }

        protected virtual IdlFunction ParseFunction(string name, string returnType, string attribute, string parameters)
        {
            return new IdlFunction(name, returnType, new IdlAttribute(attribute), this.parameterBuildStrategy.Parse(parameters));
        }

        public virtual void Build(IdlNamespaceElement element, StringBuilder typesBuilder, StringBuilder delegateBuilder, StringBuilder functionBuilder, int indent)
        {
            this.BuildFunction((IdlFunction) element, typesBuilder, delegateBuilder, functionBuilder, indent);
        }

        public virtual void BuildFunction(IdlFunction function, StringBuilder typesBuilder, StringBuilder delegateBuilder, StringBuilder functionBuilder, int indent)
        {
            var parametersBuilder = new StringBuilder();
            var bodyBuilder = new StringBuilder();

            for (var i = 0; i < function.Parameters.Count; i++)
            {
                var parameter = function.Parameters[i];

                this.parameterBuildStrategy.Build(parameter, function, parametersBuilder, bodyBuilder, indent + 1);

                if (i < function.Parameters.Count - 1)
                {
                    parametersBuilder.Append(", ");
                }
            }

            this.BuildFunctionBody(function, bodyBuilder, indent + 1);

            if (bodyBuilder.Length == 0)
            {
                bodyBuilder.Append("throw new System.NotImplementedException();".Indent(indent));
            }

            functionBuilder.AppendLine($"public static {this.MapReturnType(function.ReturnType)} {function.Name}({parametersBuilder})".Indent(indent));
            functionBuilder.AppendLine("{".Indent(indent));

            functionBuilder.AppendLine(bodyBuilder.ToString());

            functionBuilder.AppendLine("}".Indent(indent));
            functionBuilder.AppendLine();
        }

        public virtual void BuildFunctionBody(IdlFunction function, StringBuilder bodyBuilder, int indent)
        {
            // empty
        }

        protected string MapReturnType(string type)
        {
            return type switch
            {
                _ => type,
            };
        }
    }
}
