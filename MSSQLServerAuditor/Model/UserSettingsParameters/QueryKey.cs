using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSQLServerAuditor.Model.UserSettingsParameters
{
	public class QueryKey
	{
		private readonly TemplateNodeQueryInfo _queryInfo;

		public QueryKey()
		{
			this._queryInfo = null;
		}

		public QueryKey(TemplateNodeQueryInfo queryInfo) : this()
		{
			if (queryInfo == null)
			{
				throw new ArgumentNullException("queryInfo");
			}

			this._queryInfo = queryInfo;
		}

		public TemplateNodeQueryInfo Query
		{
			get { return this._queryInfo; }
		}

		public override string ToString()
		{
			string                 strQueryName = null;
			TemplateNodeLocaleInfo NodeInfo    = null;

			if (this._queryInfo != null)
			{
				if (this._queryInfo.Locales != null && this._queryInfo.Locales.Any())
				{
					NodeInfo = this._queryInfo.Locales.FirstOrDefault(
						it => it.Language == Program.Model.Settings.InterfaceLanguage
					);
				}

				if (NodeInfo != null && NodeInfo.Text != null)
				{
					strQueryName = NodeInfo.Text.Trim();
				}
				else
				{
					strQueryName = string.Format("{0} [{1}]",
						this._queryInfo.QueryName,
						this._queryInfo.Id
					);
				}
			}

			return strQueryName;
		}

		public override int GetHashCode()
		{
			int intHashCode = 0;

			if (this._queryInfo != null)
			{
				intHashCode = this._queryInfo.GetHashCode();
			}
			else
			{
				intHashCode = 0;
			}

			return intHashCode;
		}
	}
}
