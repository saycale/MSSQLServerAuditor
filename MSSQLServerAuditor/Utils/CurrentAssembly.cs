using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MSSQLServerAuditor.Utils
{
	public static class CurrentAssembly
	{
		public static string Title
		{
			get
			{
				string title = GetAttributeValue<AssemblyTitleAttribute>(a => a.Title);

				if (!string.IsNullOrEmpty(title))
				{
					return title;
				}

				return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public static string Version
		{
			get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
		}

		public static string Description
		{
			get { return GetAttributeValue<AssemblyDescriptionAttribute>(a => a.Description); }
		}

		public static string ProcessName
		{
			get { return Process.GetCurrentProcess().ProcessName; }
		}

		public static string ProcessNameBase
		{
			// get { return ProcessName.Replace(".vshost", string.Empty); }

			//
			// ticket #390: service requirenments
			//
			get
			{
				string strProcessNameBase = ProcessName;

				strProcessNameBase = strProcessNameBase.Replace(".vshost", string.Empty);

				if (strProcessNameBase.CompareTo("MSSQLServerAuditorServiceTestApp") == 0)
				{
					strProcessNameBase = "MSSQLServerAuditor";
				}

				if (strProcessNameBase.CompareTo("MSSQLServerAuditorService") == 0)
				{
					strProcessNameBase = "MSSQLServerAuditor";
				}

				return strProcessNameBase;
			}
		}

		public static string Product
		{
			get { return GetAttributeValue<AssemblyProductAttribute>(a => a.Product); }
		}

		public static string Copyright
		{
			get { return GetAttributeValue<AssemblyCopyrightAttribute>(a => a.Copyright); }
		}

		public static string Company
		{
			get { return GetAttributeValue<AssemblyCompanyAttribute>(a => a.Company); }
		}

		private static string GetAttributeValue<T>(Func<T, string> mapper) where T : Attribute
		{
			object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(T), false);

			if (attributes.Length == 0)
			{
				return string.Empty;
			}

			return mapper.Invoke((T) attributes[0]);
		}
	}
}
