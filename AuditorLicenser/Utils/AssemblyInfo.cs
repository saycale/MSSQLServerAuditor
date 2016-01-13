using System;
using System.Reflection;

namespace MSSQLServerAuditor.Licenser.Utils
{
    /// <summary>
    /// Gets the values from the AssemblyInfo.cs file for the current executing assembly
    /// </summary>
    /// <example>
    /// string company = AssemblyInfo.Company;
    /// string product = AssemblyInfo.Product;
    /// string copyright = AssemblyInfo.Copyright;
    /// string trademark = AssemblyInfo.Trademark;
    /// string title = AssemblyInfo.Title;
    /// string description = AssemblyInfo.Description;
    /// string configuration = AssemblyInfo.Configuration;
    /// string fileversion = AssemblyInfo.FileVersion;
    /// Version version = AssemblyInfo.Version;
    /// string versionFull = AssemblyInfo.VersionFull;
    /// string versionMajor = AssemblyInfo.VersionMajor;
    /// string versionMinor = AssemblyInfo.VersionMinor;
    /// string versionBuild = AssemblyInfo.VersionBuild;
    /// string versionRevision = AssemblyInfo.VersionRevision;
    /// </example>
    public class AssemblyInfo
    {
        public Assembly Assembly { get; private set; }

        public AssemblyInfo(Assembly assembly)
        {
            Assembly = assembly;
        }

        public string Company { get { return GetExecutingAssemblyAttribute<AssemblyCompanyAttribute>(a => a.Company); } }
        public string Product { get { return GetExecutingAssemblyAttribute<AssemblyProductAttribute>(a => a.Product); } }
        public string Copyright { get { return GetExecutingAssemblyAttribute<AssemblyCopyrightAttribute>(a => a.Copyright); } }
        public string Trademark { get { return GetExecutingAssemblyAttribute<AssemblyTrademarkAttribute>(a => a.Trademark); } }
        public string Title { get { return GetExecutingAssemblyAttribute<AssemblyTitleAttribute>(a => a.Title); } }
        public string Description { get { return GetExecutingAssemblyAttribute<AssemblyDescriptionAttribute>(a => a.Description); } }
        public string Configuration { get { return GetExecutingAssemblyAttribute<AssemblyDescriptionAttribute>(a => a.Description); } }
        public string FileVersion { get { return GetExecutingAssemblyAttribute<AssemblyFileVersionAttribute>(a => a.Version); } }

        public Version Version { get { return Assembly.GetExecutingAssembly().GetName().Version; } }
        public string VersionFull { get { return Version.ToString(); } }
        public string VersionMajor { get { return Version.Major.ToString(); } }
        public string VersionMinor { get { return Version.Minor.ToString(); } }
        public string VersionBuild { get { return Version.Build.ToString(); } }
        public string VersionRevision { get { return Version.Revision.ToString(); } }

        private string GetExecutingAssemblyAttribute<T>(Func<T, string> value) where T : Attribute
        {
            var attribute = (T)Attribute.GetCustomAttribute(Assembly, typeof(T));
            return value.Invoke(attribute);
        }
    }
}