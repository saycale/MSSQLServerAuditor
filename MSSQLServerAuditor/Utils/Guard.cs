using System;

namespace MSSQLServerAuditor.Utils
{
    public static class Guard
    {
        public static void ArgumentNotNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new NullReferenceException(string.Format("Argument {0} is null", argumentName));
            }
        }
    }
}