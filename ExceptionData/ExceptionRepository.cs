using System;

namespace Diagram.ExceptionData
{
    public class ExceptionRepository : Exception
    {
        public ExceptionRepository(Exception innerException, string message) :base(message, innerException){}
    }
}
