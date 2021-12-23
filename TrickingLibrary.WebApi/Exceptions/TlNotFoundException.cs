using System;

namespace TrickingLibrary.WebApi.Exceptions
{
    public class TlNotFoundException : Exception
    {
        public TlNotFoundException()
        {
        }

        public TlNotFoundException(string message)
            : base(message)
        {
        }

        public TlNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}