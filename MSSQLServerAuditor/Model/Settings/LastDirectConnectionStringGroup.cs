using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model.Settings
{
	public class InSettingsEqualityComparer : EqualityComparer<LastDirectConnectionStringGroup>
	{
		public override bool Equals(LastDirectConnectionStringGroup g1, LastDirectConnectionStringGroup g2)
		{
			if (g1 == null)
			{
				return g2 == null;
			}

			if (g1.GroupName.Equals(g2.GroupName, StringComparison.InvariantCultureIgnoreCase) &&
				g2.ConnectionStrings.TrueForAll(
					x => g1.ConnectionStrings.All(
						y => x.DataBaseType == y.DataBaseType
					)
				)
			)
			{
				return true;
			}

			return false;
		}

		public override int GetHashCode(LastDirectConnectionStringGroup obj)
		{
			return obj.GetHashCode();
		}
	}

	[Serializable]
	public class LastDirectConnectionStringGroup
	{
		public static readonly EqualityComparer<LastDirectConnectionStringGroup> InSettingsComparer = new InSettingsEqualityComparer();

		public LastDirectConnectionStringGroup()
		{
			ConnectionStrings = new List<LastDirectConnectionString>();
		}

		/// <summary>
		/// Connection string.
		/// </summary>
		[XmlElement(ElementName = "ConnectionStrings")]
		public List<LastDirectConnectionString> ConnectionStrings { get; set; }

		/// <summary>
		/// Group name.
		/// </summary>
		[XmlElement(ElementName = "GroupName")]
		public string GroupName { get; set; }

		/// <summary>
		/// Group name for combobox.
		/// </summary>
		[XmlIgnore]
		public string ConnectionString {
			get { return GroupName; }
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is LastDirectConnectionStringGroup))
			{
				return false;
			}

			var group = obj as LastDirectConnectionStringGroup;

			if (group.GroupName != this.GroupName)
			{
				return false;
			}

			if (this.ConnectionStrings.Count != group.ConnectionStrings.Count)
			{
				return false;
			}

			if (!this.ConnectionStrings.TrueForAll(x=>group.ConnectionStrings.Any(y =>
				y.IsODBC == x.IsODBC &&
				y.ConnectionString == x.ConnectionString &&
				y.Name == x.Name
			)))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public override int GetHashCode()
		{
			var hash = "ConnectionString" + (ConnectionString ?? String.Empty) + "GroupName" + (GroupName ?? String.Empty);

			if (ConnectionStrings != null && ConnectionStrings.Count != 0)
			{
				ConnectionStrings.ForEach(
					el => hash += el == null ? String.Empty
						: ("el.Name" + el.Name + "el.ConnectionString" + el.ConnectionString + "el.IsODBC" + el.IsODBC.ToString()));
			}

			return hash.GetHashCode();
		}
	}
}
