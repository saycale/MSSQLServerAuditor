using System;

namespace MSSQLServerAuditor.Model
{
	public class InstanceVersion : IComparable
	{
		public int Major { get; set; }
		public int Minor { get; set; }
		public int Build { get; set; }

		#region Implementation of IComparable

		public int CompareTo(object obj)
		{
			var version = (InstanceVersion)obj;

			if (Major != version.Major)
			{
				return Major > version.Major ? 1 : -1;
			}

			if (Minor != version.Minor)
			{
				return Minor > version.Minor ? 1 : -1;
			}

			if (Build != version.Build)
			{
				return Build > version.Build ? 1 : -1;
			}

			return 0;
		}

		public override bool Equals(object obj)
		{
			InstanceVersion version = obj as InstanceVersion;

			if (version == null)
			{
				return false;
			}

			return version.CompareTo(this) == 0;
		}

		public override int GetHashCode()
		{
			int hash = Major;

			hash = (hash * 397) ^ Minor.GetHashCode();
			hash = (hash * 397) ^ Build.GetHashCode();

			return hash;
		}

		#endregion

		public static InstanceVersion GetMinVersion(string version)
		{
			version = version.Trim();

			if(version == "*")
			{
				return new InstanceVersion() { Major = int.MinValue};
			}

			var result = new InstanceVersion();
			var split  = version.Split(' ')[0].Split('.');

			if(split.Length > 0)
			{
				result.Major = (split[0] ?? "0") == "*" ? int.MinValue : int.Parse(split[0] ?? "0");
			}

			if(split.Length > 1)
			{
				result.Minor = (split[1] ?? "0") == "*" ? int.MinValue : int.Parse(split[1] ?? "0");
			}

			if(split.Length > 2)
			{
				result.Build = (split[2] ?? "0") == "*" ? int.MinValue : int.Parse(split[2] ?? "0");
			}

			return result;
		}

		public static InstanceVersion GetMaxVersion(string version)
		{
			version = version.Trim();

			if (version == "*")
			{
				return new InstanceVersion() { Major = int.MaxValue };
			}

			var result = new InstanceVersion();
			var split  = version.Split(' ')[0].Split('.');

			if (split.Length > 0)
			{
				result.Major = (split[0] ?? "0") == "*" ? int.MaxValue : int.Parse(split[0] ?? "0");
			}

			if (split.Length > 1)
			{
				result.Minor = (split[1] ?? "0") == "*" ? int.MaxValue : int.Parse(split[1] ?? "0");
			}

			if (split.Length > 2)
			{
				result.Build = (split[2] ?? "0") == "*" ? int.MaxValue : int.Parse(split[2] ?? "0");
			}

			return result;
		}

		#region Overrides of Object

		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}",
				Major,
				Minor,
				Build
			);
		}

		#endregion

		public InstanceVersion()
		{
			this.Major = 0;
			this.Minor = 0;
			this.Build = 0;
		}

		public InstanceVersion(string strVersion)
		{
			string strMyVersion = strVersion;

			if (string.IsNullOrEmpty(strMyVersion))
			{
				this.Major = 0;
				this.Minor = 0;
				this.Build = 0;
			}
			else
			{
				strMyVersion = strMyVersion.Trim();
				var split = strMyVersion.Trim().Split(' ')[0].Split('.');

				this.Major = int.Parse(split[0] ?? "0");
				this.Minor = int.Parse(split[1] ?? "0");
				this.Build = int.Parse(split[2] ?? "0");
			}
		}
	}
}
