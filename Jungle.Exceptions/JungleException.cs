using System;
using System.Runtime.Serialization;

namespace Jungle.Exceptions
{
    public class JungleException : ApplicationException
    {
        public JungleException()
        {
        }

        public JungleException(string message) : base(message)
        {
        }

        public JungleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected JungleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
