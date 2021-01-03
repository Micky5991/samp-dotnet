using System.Collections.Generic;
using System.Text;

namespace Micky5991.Samp.Net.Generators
{
    public class BuilderTargetCollection : Dictionary<BuilderTarget, StringBuilder>
    {

        public void Add(BuilderTarget name)
        {
            this.Add(name, new StringBuilder());
        }

    }
}
