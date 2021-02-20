using System;

namespace Micky5991.Samp.Net.Core.Exceptions
{
    public class InvalidThreadException : Exception
    {
        public InvalidThreadException(string message)
            : base(message)
        {
        }
    }
}
