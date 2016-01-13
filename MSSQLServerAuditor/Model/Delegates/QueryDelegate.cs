using System;
using System.Collections.Generic;
using System.Data;

namespace MSSQLServerAuditor.Model.Delegates
{
	public delegate DataTable[] BaseResolverDelegate(
		InstanceInfo                    iinfo,
		QueryItemInfo                   qinfo,
		string                          str,
		IEnumerable<QueryParameterInfo> parameters,
		IEnumerable<ParameterValue>     parametersValues,
		ProgressItem                    progress,
		bool                            boolInner
	);

	public delegate QueryDatabaseResultInfo BaseResolverQueryItemDelegate(
		InstanceInfo             iinfo,
		QueryItemInfo            qinfo,
		string                   str,
		string                   str1,
		List<QueryParameterInfo> parameters,
		List<ParameterValue>     parametersValues,
		ProgressItem             progress
	);
}
