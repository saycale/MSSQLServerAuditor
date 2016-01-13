using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlCodeGuardAPI;

namespace MSSQLServerAuditor.Utils
{
	public class CodeGuardCompleteEventArgs: EventArgs
	{
		public IEnumerable<Issue> Issues;

		public CodeGuardCompleteEventArgs(IEnumerable<Issue> issues)
		{
			Issues = issues;
		}
	}
}
