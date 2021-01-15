using System.Collections.Generic;

namespace Micky5991.Samp.Net.Generators.Data
{
    public class IdlCallback : IdlFunction
    {
        public IdlCallback(string name, string returnType, IdlAttribute attribute, IList<IdlFunctionParameter> parameters)
            : base(name, returnType, attribute, parameters)
        {
        }
    }
}
