using System;

namespace MSSQLServerAuditor.Model
{
    public class InvalidTemplateException: Exception
    {
        public InvalidTemplateException(string message): base(message)
        {
        }
    }
}