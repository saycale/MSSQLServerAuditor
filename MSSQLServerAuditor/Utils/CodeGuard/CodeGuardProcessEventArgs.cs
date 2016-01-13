using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlCodeGuardAPI;

namespace MSSQLServerAuditor.Utils
{
	/// <summary>
	/// EventArgs for <c>CodeGuardAnalyzer</c> events
	/// </summary>
	public class CodeGuardProcessEventArgs: EventArgs
	{
		public DatabaseObject Obj;
		public int Total;
		public int Current;

		public CodeGuardProcessEventArgs(DatabaseObject obj, int total, int current)
		{
			Obj = obj;
			Total = total;
			Current = current;
		}
	}
}
