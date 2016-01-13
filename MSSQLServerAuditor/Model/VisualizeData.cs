using System;
using System.Collections.Generic;
using MSSQLServerAuditor.Preprocessor;

namespace MSSQLServerAuditor.Model
{
	/// <summary>
	/// Visualize data
	/// </summary>
	class VisualizeData
	{
		/// <summary>
		/// Contents of source Xml result file
		/// </summary>
		public string SourceXml { get; set; }

		/// <summary>
		/// Preprocessor results
		/// </summary>
		public List<PreprocessorAreaData> PreprocessorAreas { get; set; }

		/// <summary>
		/// Last update node datetime.
		/// </summary>
		public DateTime? NodeLastUpdated { get; set; }

		/// <summary>
		/// Last update node duration.
		/// </summary>
		public DateTime? NodeLastUpdateDuration { get; set; }
	}
}
