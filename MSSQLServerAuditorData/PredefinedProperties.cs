using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MSSQLServerAuditor")]
namespace MSSQLServerAuditor.Data
{
    internal class PredefinedProperties
    {
        public static string PublicKeySign =  "TEST KEY (NOT VALID!)";
        public static string PrivateKeyDecrypt = "TEST KEY (NOT VALID!)";
    }
}
