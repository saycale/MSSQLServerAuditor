using System;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model.Settings
{
	[Serializable]
	public class PostBuildSqliteDb
	{
		private string _postBuildScript;

		[XmlAttribute("Alias")]
		public string Alias { get; set; }

		[XmlAttribute("FileName")]
		public string FileName { get; set; }

		/// <summary>
		/// SQLite db name script
		/// </summary>
		[XmlText]
		public string PostBuildScript
		{
			get { return this._postBuildScript; }
			set { this._postBuildScript = value; }
		}
	}
}
