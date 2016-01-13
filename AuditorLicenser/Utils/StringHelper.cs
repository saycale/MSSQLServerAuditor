using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSSQLServerAuditor.Licenser.Utils
{
    public static class StringHelper
    {
        public static string GetRelativePath(this string path)
        {
            Uri file = new Uri(path);
            // Must end in a slash to indicate folder
            Uri folder = new Uri(Application.StartupPath+"\\");
            string relativePath =
            Uri.UnescapeDataString(
                folder.MakeRelativeUri(file)
                    .ToString()
                    .Replace('/', Path.DirectorySeparatorChar)
                );
            if (string.IsNullOrEmpty(relativePath))
                relativePath = ".";
            return relativePath;
        }
    }
}
