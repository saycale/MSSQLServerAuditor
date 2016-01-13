using System.Text.RegularExpressions;

namespace MSSQLServerAuditor.Utils
{
    public class HexHelper
    {
        private static readonly Regex R = new Regex(@"^[0-9A-F\r\n]+$");

        public static bool VerifyHex(string hex)
        {
            return R.Match(hex).Success;
        }
    }
}