using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using log4net;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Properties;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Gui
{
    /// <summary>
    /// Dialog of components in program.
    /// </summary>
    public partial class frmComponents : LocalizableForm
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializing object frmComponents.
        /// </summary>
        public frmComponents()
        {
            string isDebug = string.Empty;
            if (AppVersionHelper.IsDebug())
            {
                isDebug = " DEBUG";
            }

            InitializeComponent();

            this.Text = String.Format(GetLocalizedText("captionText"), CurrentAssembly.Title) + isDebug;

            textBoxDescription.Font = new Font(FontFamily.GenericMonospace, textBoxDescription.Font.Size);

            textBoxDescription.Text = BuildDescripionFieldText();
        }

        #region Formation of the text for the Description field / Формирование текста для поля Description

        string BuildDescripionFieldText()
        {
            // log.Debug("var loadedModules = GetLoadedModules();");
            var loadedModules = GetLoadedModules();

            // log.Debug("var winVersion = GetWinVersion();");
            var winVersion = GetWinVersion();

            // log.Debug("var frameworksInstalled = GetFrameworksInstalled();");
            var frameworksInstalled = GetFrameworksInstalled();

            // log.Debug("var adoVersion = GetAdoVersion();");
            var adoVersion = GetAdoVersion();

            // log.Debug("var adoVersion = GetAdoVersion();");
            var ieVersion = GetIeVersion();

            return string.Format(
            	Program.Model.LocaleManager.GetLocalizedText("frmComponents", "descriptionStringPattern"),
                loadedModules,
                winVersion,
                frameworksInstalled,
                adoVersion,
                ieVersion);
        }

        private static string GetFrameworksInstalled()
        {
            List<FrameworkVersions.VersionInfo> fws;

            // log.Debug("var f1ws = FrameworkVersions.GetInstalledVersions().ToList();");
            try
            {
                fws = FrameworkVersions.GetInstalledVersions().ToList();
            }
            catch (Exception ex)
            {
                return string.Format(Program.Model.LocaleManager.GetLocalizedText("frmComponents", "frameworkVersionErrorPattern"),
            	                     ex.Message);
            }

            // log.Debug("return string.Join(Nl, fws.OrderBy(fw => fw.Version).Select(fw => fw.ToString()));");

            return string.Join(Environment.NewLine, fws.OrderBy(fw => fw.Version).Select(fw => fw.ToString()));
        }

        /// <summary>
        /// Get version ADO.
        /// </summary>
        /// <returns></returns>
        public static string GetAdoVersion()
        {
            return Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\DataAccess", "FullInstallVer", "unknown").ToString();
        }

        /// <summary>
        /// Get version IE.
        /// </summary>
        /// <returns>Return version IE.</returns>
        public static string GetIeVersion()
        {
            return Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Internet Explorer", "Version", "unknown").ToString();
        }

        /// <summary>
        /// Get windows version.
        /// </summary>
        /// <returns>Return windows version.</returns>
        public static string GetWinVersion()
        {
            return Environment.OSVersion + " " + (Environment.Is64BitOperatingSystem ? "x64" : string.Empty);
        }

        private string GetLoadedModules()
        {
            var sb = new StringBuilder();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var name = assembly.GetName();

                sb.AppendLine(
                    string.Format("{0,-57}{1,-10}{2,7}", name.Name, "v." + name.Version, name.ProcessorArchitecture));
                //assembly.GetName().ProcessorArchitecture.ToString()

                //string assemblyName = assembly.GetName().Name;
                //    strings += Environment.NewLine;
                //    strings += assemblyName + " (" + assembly.GetName().Version + ")";
            }
            return sb.ToString();
        }

        #endregion
    }
}
