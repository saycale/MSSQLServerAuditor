using System.Collections.Generic;
using System.Data;
using System.Linq;
using log4net;
using MSSQLServerAuditor.BusinessLogic;
using MSSQLServerAuditor.SQLite.Common;

namespace MSSQLServerAuditor.Model
{
	public partial class TemplateNodeInfo
	{
		public class NodeAttirbutes
		{
			private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
			public Dictionary<string, string>    Values;
			private bool                         _Initialized { get; set; }
			private TemplateNodeInfo             _NodeInstance { get; set; }

			public NodeAttirbutes()
			{
				this.Values        = new Dictionary<string, string>();
				this._NodeInstance = null;
				this._Initialized  = false;
			}

			public NodeAttirbutes(TemplateNodeInfo nodeInstance, DataRow dataRow = null) : this()
			{
				this._NodeInstance = nodeInstance;

				if (dataRow != null)
				{
					for (var i = 0; i < dataRow.Table.Columns.Count; i++)
					{
						var name = dataRow.Table.Columns[i].ColumnName;
						var value = dataRow[i].ToString();
						Values.Add(name, value);
					}

					this._Initialized = true;
				}
			}

			public string GetUName()
			{
				if (!this._Initialized)
				{
					return null;
				}

				return GetValue("NodeUName");
			}

			public string GetUIcon()
			{
				string result = string.Empty;

				if (Values.TryGetValue("NodeUIcon", out result))
				{
					return result;
				}

				return string.Empty;
			}

			public string GetUId()
			{
				if (!this._Initialized)
				{
					return null;
				}

				return GetValue("NodeUId", true);
			}

			public string GetValue(string name, bool throwException = false)
			{
				string result = string.Empty;

				if (Values.TryGetValue(name, out result))
				{
					return result ;
				}

				var message = "No [" + name + "] " + this._NodeInstance.ReplicationSourceQuery;

				if (throwException)
				{
					log.Error(message);
					throw new InvalidTemplateException(message);
				}

				// log.Debug(message);
				return null;
			}

			public bool? GetEnabledValue()
			{
				string result = string.Empty;

				return Values.TryGetValue("NodeEnabled", out result)
					? (bool?)SqLiteBool.FromString(result) : null;
			}
		}
	}
}
