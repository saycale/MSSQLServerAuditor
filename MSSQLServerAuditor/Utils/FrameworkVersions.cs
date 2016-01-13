using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Win32;
using log4net;

namespace MSSQLServerAuditor.Utils
{
    /// <summary>
    /// Version framework.
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public static class FrameworkVersions
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Struct version info.
        /// </summary>
        public struct VersionInfo
        {
            /// <summary>
            /// Install version.
            /// </summary>
            public readonly string Install;
            /// <summary>
            /// Version.
            /// </summary>
            public readonly string Version;
            /// <summary>
            /// Profile.
            /// </summary>
            public readonly string Profile;
            /// <summary>
            /// SP.
            /// </summary>
            public readonly string SP;

            /// <summary>
            /// Initializing object VersionInfo.
            /// </summary>
            /// <param name="version">Version.</param>
            /// <param name="profile">Profile.</param>
            /// <param name="sp">SP.</param>
            /// <param name="install">Install version.</param>
            public VersionInfo(string version, string profile, string sp, string install)
            {
                Install = install;
                Version = version;
                Profile = profile;
                SP = sp;
            }

            /// <summary>
            /// Converts a string to an equivalent
            /// </summary>
            /// <returns>About framework.</returns>
            public override string ToString()
            {
                var s = "v." + Version;

                if (!string.IsNullOrWhiteSpace(Profile))
                    s = s + " " + Profile + " profile";

                if (!string.IsNullOrWhiteSpace(SP))
                    s = s + " SP" + SP;

                return s;
            }
        }

        /// <summary>
        /// Gets the installed version Framework.
        /// </summary>
        /// <returns>List info version.</returns>
        public static IEnumerable<VersionInfo> GetInstalledVersions()
        {
            // log.Debug("using (var lmKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))");
            using (var lmKey = Registry.LocalMachine)// RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
            {
                // log.Debug("using (var ndpKey = lmKey.OpenSubKey(...)");
                using (var ndpKey = lmKey.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
                {
                    if (ndpKey == null)
                        yield break;

                    // log.Debug("var subkeys = ndpKey.GetSubKeyNames().ToList();");
                    var subkeys = ndpKey.GetSubKeyNames().ToList();

                    foreach (var versionKeyName in subkeys)
                    {
                        if (!versionKeyName.StartsWith("v"))
                            continue;

                        var versionKey = ndpKey.OpenSubKey(versionKeyName);
                        var name = (string) versionKey.GetValue("Version", "");
                        var sp = versionKey.GetValue("SP", "").ToString();
                        var install = versionKey.GetValue("Install", "").ToString();

                        if (install == "") //no install info, ust be later
                        {
                            //Console.WriteLine(versionKeyName + "  " + name);
                        }
                        else
                        {
                            if (sp != "" && install == "1")
                            {
                                //Console.WriteLine(versionKeyName + "  " + name + "  SP" + sp);
                                yield return new VersionInfo(name, "", sp, install);
                            }
                        }

                        if (name != "")
                        {
                            continue;
                        }

                        foreach (var subKeyName in versionKey.GetSubKeyNames())
                        {
                            var subKey = versionKey.OpenSubKey(subKeyName);
                            if (subKey == null)
                                continue;

                            name = (string) subKey.GetValue("Version", "");

                            if (name != "")
                                sp = subKey.GetValue("SP", "").ToString();

                            install = subKey.GetValue("Install", "").ToString();

                            if (install == "") //no install info, ust be later
                            {
                                //Console.WriteLine(versionKeyName + "  " + name);
                            }
                            else
                            {
                                if (sp != "" && install == "1")
                                {
                                    //Console.WriteLine("  " + subKeyName + "  " + name + "  SP" + sp);
                                    yield return new VersionInfo(name, subKeyName, sp, install);
                                }
                                else if (install == "1")
                                {
                                    //Console.WriteLine("  " + subKeyName + "  " + name);
                                    yield return new VersionInfo(name, subKeyName, sp, install);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}