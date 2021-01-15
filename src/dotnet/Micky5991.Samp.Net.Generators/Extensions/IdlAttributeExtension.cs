using Micky5991.Samp.Net.Generators.Data;

namespace Micky5991.Samp.Net.Generators.Extensions
{
    public static class IdlAttributeExtension
    {
        public static bool IsOut(this IdlAttribute attribute)
        {
            return attribute.TryGetValue("out", out _);
        }

        public static bool IsIn(this IdlAttribute attribute)
        {
            return attribute.TryGetValue("in", out _);
        }

        public static bool IsInAndOut(this IdlAttribute attribute)
        {
            return attribute.IsIn() && attribute.IsOut();
        }
    }
}
