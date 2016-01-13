using System;

namespace MSSQLServerAuditor.Graph
{
	/// <summary>
	/// Plain data item for Graph
	/// </summary>
	public struct PlainItem
	{
		/// <summary>
		/// Date of activity for DataTime graphs
		/// </summary>
		public DateTime DateTime;

		/// <summary>
		/// X Value of activity for XY graphs
		/// </summary>
		public double? XValue;

		/// <summary>
		/// Name of activity
		/// </summary>
		public String Name;

		/// <summary>
		/// YValue to show on the graph. It can be value of elapsed seconds, file size in bytes etc...
		/// </summary>
		public double YValue;
	}
}
