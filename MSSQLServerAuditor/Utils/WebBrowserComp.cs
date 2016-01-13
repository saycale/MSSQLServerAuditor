using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using log4net;

namespace MSSQLServerAuditor.Utils
{
	static class WebBrowserComp
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public static bool SetIeComp(WebBrowser webBrowser)
		{
			String appname = Process.GetCurrentProcess().ProcessName + ".exe";

			// make the control is not running inside Visual Studio Designer
			if (String.Compare(appname, "devenv.exe", true) == 0 || String.Compare(appname, "XDesProc.exe", true) == 0)
			{
				return true;
			}

			//
			// http://msdn.microsoft.com/en-us/library/ee330720(v=vs.85).aspx
			//
			try
			{
				SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION",                appname, GetBrowserEmulationMode());
				SetBrowserFeatureControlKey("FEATURE_AJAX_CONNECTIONEVENTS",            appname, 1);
				SetBrowserFeatureControlKey("FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", appname, 1);
				SetBrowserFeatureControlKey("FEATURE_MANAGE_SCRIPT_CIRCULAR_REFS",      appname, 1);
				SetBrowserFeatureControlKey("FEATURE_DOMSTORAGE",                       appname, 1);
				SetBrowserFeatureControlKey("FEATURE_GPU_RENDERING",                    appname, 1);
				SetBrowserFeatureControlKey("FEATURE_IVIEWOBJECTDRAW_DMLT9_WITH_GDI",   appname, 0);
				SetBrowserFeatureControlKey("FEATURE_DISABLE_LEGACY_COMPRESSION",       appname, 1);
				SetBrowserFeatureControlKey("FEATURE_LOCALMACHINE_LOCKDOWN",            appname, 0);
				SetBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_OBJECT",                 appname, 0);
				SetBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_SCRIPT",                 appname, 0);
				SetBrowserFeatureControlKey("FEATURE_DISABLE_NAVIGATION_SOUNDS",        appname, 1);
				SetBrowserFeatureControlKey("FEATURE_SCRIPTURL_MITIGATION",             appname, 1);
				SetBrowserFeatureControlKey("FEATURE_SPELLCHECKING",                    appname, 0);
				SetBrowserFeatureControlKey("FEATURE_STATUS_BAR_THROTTLING",            appname, 1);
				SetBrowserFeatureControlKey("FEATURE_TABBED_BROWSING",                  appname, 1);
				SetBrowserFeatureControlKey("FEATURE_VALIDATE_NAVIGATE_URL",            appname, 1);
				SetBrowserFeatureControlKey("FEATURE_WEBOC_DOCUMENT_ZOOM",              appname, 1);
				SetBrowserFeatureControlKey("FEATURE_WEBOC_POPUPMANAGEMENT",            appname, 0);
				SetBrowserFeatureControlKey("FEATURE_WEBOC_MOVESIZECHILD",              appname, 1);
				SetBrowserFeatureControlKey("FEATURE_ADDON_MANAGEMENT",                 appname, 0);
				SetBrowserFeatureControlKey("FEATURE_WEBSOCKET",                        appname, 1);
				SetBrowserFeatureControlKey("FEATURE_WINDOW_RESTRICTIONS ",             appname, 0);
				SetBrowserFeatureControlKey("FEATURE_XMLHTTP",                          appname, 1);
			}
			catch (Exception ex)
			{
				log.Error(ex);

				return false;
			}

			// RegistryKey RKCU8 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree);
			//
			// int         value9 = 9999;
			// int         value8 = 8888;
			// Version     ver = null;
			//
			// if (webBrowser != null)
			// {
			// 	ver = webBrowser.Version;
			// }
			// else
			// {
			// 	ver    = new Version("8.0");
			// 	value8 = 0x1F40;
			// }
			//
			// int value = value9;
			//
			// try
			// {
			// 	string[] parts = ver.ToString().Split('.');
			// 	int      vn = 0;
			//
			// 	int.TryParse(parts[0], out vn);
			//
			// 	if (vn != 0)
			// 	{
			// 		if (vn == 9)
			// 		{
			// 			value = value9;
			// 		}
			// 		else
			// 		{
			// 			value = value8;
			// 		}
			// 	}
			// }
			// catch (Exception ex)
			// {
			// 	log.Error(ex);
			//
			// 	value = value9;
			// }
			//
			// // Setting the key in LocalMachine
			// if (RKCU8 != null)
			// {
			// 	try
			// 	{
			// 		RKCU8.SetValue(appname, value, RegistryValueKind.DWord);
			// 		RKCU8.Close();
			// 	}
			// 	catch (Exception ex)
			// 	{
			// 		log.Error(ex);
			// 		return false;
			// 	}
			// }

			if (webBrowser != null)
			{
				// TODO: need to reload component after compatibility changing
			}

			return true;
		}

		private static void SetBrowserFeatureControlKey(string feature, string appName, uint value)
		{
			using (var key = Registry.CurrentUser.CreateSubKey(
					String.Concat(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\", feature),
					RegistryKeyPermissionCheck.ReadWriteSubTree
					)
				)
			{
				key.SetValue(appName, (UInt32)value, RegistryValueKind.DWord);
			}
		}

		private static UInt32 GetBrowserEmulationMode()
		{
			int browserVersion = 7;

			using (var ieKey = Registry.LocalMachine.OpenSubKey(
					@"SOFTWARE\Microsoft\Internet Explorer",
					RegistryKeyPermissionCheck.ReadSubTree,
					System.Security.AccessControl.RegistryRights.QueryValues
					)
				)
			{
				var version = ieKey.GetValue("svcVersion");

				if (version == null)
				{
					version = ieKey.GetValue("Version");

					if (version == null)
					{
						throw new ApplicationException("Microsoft Internet Explorer is required!");
					}
				}

				int.TryParse(version.ToString().Split('.')[0], out browserVersion);
			}

			// Internet Explorer 11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 Standards mode.
			// Default value for Internet Explorer 11.
			UInt32 mode = 11000;

			switch (browserVersion)
			{
				case 7:
					// Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode.
					// Default value for applications hosting the WebBrowser Control.
					mode = 7000;
					break;
				case 8:
					// Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode.
					// Default value for Internet Explorer 8
					mode = 8000;
					break;
				case 9:
					// Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode.
					// Default value for Internet Explorer 9.
					mode = 9000;
					break;
				case 10:
					// Internet Explorer 10. Webpages containing standards-based !DOCTYPE directives are displayed in IE10 mode.
					// Default value for Internet Explorer 10.
					mode = 10000;
					break;
				default:
					// Internet Explorer 11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 mode.
					// Default value for Internet Explorer 11.
					mode = 11000;
					break;
			}

			return mode;
		}
	}
}
