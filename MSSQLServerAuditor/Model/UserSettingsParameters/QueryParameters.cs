using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MSSQLServerAuditor.BusinessLogic.LocalStorage;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables.UserSettings;

namespace MSSQLServerAuditor.Model.UserSettingsParameters
{
	class QueryParameters:INotifyPropertyChanged, IQueryParameters
	{
		private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		#region Members
		public           event PropertyChangedEventHandler                      PropertyChanged;
		private          bool                                                   _availableEdit;
		private          bool                                                   _availableRemove;
		private          List<ParameterInfoLocalizable>                         _selectedParametes;
		private          BindingList<ParameterInfoLocalizable>                  _prameters;
		private          QueryKey                                               _selectedQuery;
		private          BindingList<QueryKey>                                  _queries;
		private readonly ConcreteTemplateNodeDefinition                         _ctNodeDefinition;
		private readonly ConcreteTemplateNodeDefinition                         _savedNode;
		private readonly IDictionary<QueryKey, IList<ParameterInfoLocalizable>> _dicParameters;
		private readonly Action                                                 _updateAction;
		#endregion

		#region Constructor
		public QueryParameters()
		{
			this._availableEdit     = false;
			this._availableRemove   = false;
			this._selectedParametes = new List<ParameterInfoLocalizable>();
			this._prameters         = new BindingList<ParameterInfoLocalizable>();
			this._selectedQuery     = null;
			this._queries           = new BindingList<QueryKey>();
			this._ctNodeDefinition = null;
			this._savedNode        = null;
			this._dicParameters    = new Dictionary<QueryKey, IList<ParameterInfoLocalizable>>();
			this._updateAction     = null;
		}

		public QueryParameters(ConcreteTemplateNodeDefinition nodeDefinition, ConcreteTemplateNodeDefinition savedNodeDefinition, Action updateAction) : this()
		{
			if (nodeDefinition == null)
			{
				throw new ArgumentNullException("nodeDefinition");
			}

			this._ctNodeDefinition = nodeDefinition;
			this._savedNode        = savedNodeDefinition;
			this._updateAction     = updateAction;
		}
		#endregion

		#region IQueryParameters
		public void Init()
		{
			IStorageManager storageManager = Program.Model.GetVaultProcessor(this._ctNodeDefinition.Connection);
			CurrentStorage  storage        = storageManager.CurrentStorage;

			this._ctNodeDefinition.TemplateNode.LoadUserParameters(storage);

			ParseNode(this._ctNodeDefinition.TemplateNode);

			if(this._dicParameters.Any())
			{
				Queries.RaiseListChangedEvents = false;
				Queries.Clear();

				foreach (var query in this._dicParameters.Keys)
				{
					Queries.Add(query);
				}

				Queries.RaiseListChangedEvents = true;

				var fquery = Queries.FirstOrDefault();

				if (fquery != null)
				{
					SelectedQuery = fquery;
				}
			}
		}

		public BindingList<QueryKey> Queries
		{
			get { return this._queries; }

			set
			{
				if (value != this._queries)
				{
					this._queries = value;

					OnPropertyChanged("Queries");
				}
			}
		}

		public QueryKey SelectedQuery
		{
			get { return this._selectedQuery; }

			set
			{
				if (value != null)
				{
					if (value != this._selectedQuery)
					{
						this._selectedQuery = value;

						SetSelectedQuery(this._selectedQuery);
						OnPropertyChanged("SelectedQuery");
					}
				}
			}
		}

		public BindingList<ParameterInfoLocalizable> Parameters
		{
			get { return this._prameters; }

			set
			{
				if (value != this._prameters)
				{
					this._prameters = value;
					OnPropertyChanged("Parameters");
				}
			}
		}

		public string Name
		{
			get
			{
				var name = this._ctNodeDefinition.TemplateNode.Locales.FirstOrDefault(
					it => it.Language == Program.Model.Settings.InterfaceLanguage
				);

				return name != null ? name.Text : this._ctNodeDefinition.TemplateNode.Title;
			}
		}

		public void SetSelectedParametes(IEnumerable<ParameterInfoLocalizable> parameters)
		{
			if (parameters != null)
			{
				if (parameters != this._selectedParametes)
				{
					this._selectedParametes.Clear();
					this._selectedParametes.AddRange(parameters);
				}
			}
		}

		public void RemoveSelectedParameters()
		{
			foreach (var parameter in this._selectedParametes)
			{
				if (parameter.Type == ParameterInfoType.EditableParameter)
				{
					Parameters.Remove(parameter);
				}
			}
		}

		public void AddParameter()
		{
			var parameter = new ParameterInfoLocalizable(Parameters[0].Key, ParameterInfoType.EditableParameter, "", null, null);

			Parameters.Add(parameter);
			AvailableRemove = true;
		}

		public void ApplyChanged()
		{
			if (this._ctNodeDefinition != null)
			{
				UpdateQueryParameters(this._ctNodeDefinition.TemplateNode);
				SaveQueryParameters(this._savedNode, this._ctNodeDefinition.Connection);
			}
		}

		public bool AvailbleEdit
		{
			get { return this._availableEdit; }

			set
			{
				if (value != this._availableEdit)
				{
					this._availableEdit = value;
					OnPropertyChanged("AvailbleEdit");
				}
			}
		}

		public bool AvailableRemove
		{
			get { return this._availableRemove; }

			set
			{
				if (value != this._availableRemove)
				{
					this._availableRemove = value;
					OnPropertyChanged("AvailableRemove");
				}
			}
		}

		public bool IsHideTypeColumn
		{
			get
			{
				bool isHideTypeColumn = false;

				if (SelectedQuery != null && SelectedQuery.Query != null)
				{
					isHideTypeColumn = SelectedQuery.Query.IsHideTypeColumn;
				}

				return isHideTypeColumn;
			}
		}

		public bool IsHideTabs
		{
			get
			{
				bool isHideTabs = false;

				if (SelectedQuery != null && SelectedQuery.Query != null)
				{
					isHideTabs = SelectedQuery.Query.IsHideTabs;
				}

				return isHideTabs;
			}
		}

		public void TryUpdate()
		{
			try
			{
				if (this._updateAction != null)
				{
					_updateAction();
				}
			}
			catch (Exception ex)
			{
				this._log.Error("TryUpdate exception", ex);
			}
		}
		#endregion

		#region Service methods
		private void SetSelectedQuery(QueryKey queryName)
		{
			if (this._dicParameters.ContainsKey(queryName))
			{
				Parameters.RaiseListChangedEvents = false;
				Parameters.Clear();

				foreach (var parameterInfo in this._dicParameters[queryName])
				{
					Parameters.Add(parameterInfo);
				}

				Parameters.RaiseListChangedEvents = true;

				this._selectedParametes = new List<ParameterInfoLocalizable>();

				AvailbleEdit = queryName.Query is TemplateNodeSqlGuardQueryInfo;

				AvailableRemove = false;
			}
		}

		private void ParseNode(TemplateNodeInfo node)
		{
			foreach (var queryInfo in node.Queries.Where(info => info.ParameterValues.Any()))
			{
				this._dicParameters.Add(ParseQuery(queryInfo));
			}

			foreach (var queryInfo in node.GroupQueries.Where(info => info.ParameterValues.Any()))
			{
				this._dicParameters.Add(ParseQuery(queryInfo));
			}

			foreach (var queryInfo in node.SqlCodeGuardQueries)
			{
				this._dicParameters.Add(ParseQuery(queryInfo));
			}
		}

		private KeyValuePair<QueryKey, IList<ParameterInfoLocalizable>> ParseQuery(TemplateNodeQueryInfo queryInfo)
		{
			var queryParameters = new List<ParameterInfoLocalizable>();

			if (queryInfo is TemplateNodeSqlGuardQueryInfo)
			{
				var guardQueryInfo = queryInfo as TemplateNodeSqlGuardQueryInfo;

				queryParameters.Add(new ParameterInfoLocalizable(queryInfo.IdsHierarchy,
					ParameterInfoType.Attribute, "QueryCodeColumn", guardQueryInfo.QueryCodeColumn, null));
				queryParameters.Add(new ParameterInfoLocalizable(queryInfo.IdsHierarchy,
					ParameterInfoType.Attribute, "QueryObjectColumn", guardQueryInfo.QueryObjectColumn, null));
				queryParameters.Add(new ParameterInfoLocalizable(queryInfo.IdsHierarchy,
					ParameterInfoType.Attribute, "IncludedIssue", guardQueryInfo.IncludedIssue, null));
				queryParameters.Add(new ParameterInfoLocalizable(queryInfo.IdsHierarchy,
					ParameterInfoType.Attribute, "ExcludedIssue", guardQueryInfo.ExcludedIssue, null));
				queryParameters.Add(new ParameterInfoLocalizable(queryInfo.IdsHierarchy,
					ParameterInfoType.Attribute, "AddSummary", guardQueryInfo.AddSummary.ToString(), null));
			}

			foreach (ParameterValue paramValue in queryInfo.ParameterValues)
			{
				ParameterInfoLocalizable paramInfo = new ParameterInfoLocalizable(
					queryInfo.IdsHierarchy,
					ParameterInfoType.Parameter,
					paramValue.Name,
					paramValue.StringValue,
					paramValue.UserValue,
					paramValue.Locales
				);

				queryParameters.Add(paramInfo);
			}

			return new KeyValuePair<QueryKey, IList<ParameterInfoLocalizable>>(
				new QueryKey(queryInfo),queryParameters);
		}

		private void UpdateQueryParameters(TemplateNodeInfo templateNode)
		{
			IEnumerable<TemplateNodeQueryInfo> allQueries =
				templateNode.Queries.Union(templateNode.GroupQueries);

			var allParameters = this._dicParameters.SelectMany(el => el.Value).ToArray();

			foreach (TemplateNodeQueryInfo tnQi in allQueries)
			{
				foreach (ParameterValue parameterValue in tnQi.ParameterValues)
				{
					ParameterInfo parameterInfo =
						allParameters.FirstOrDefault(p => p.Parameter == parameterValue.Name);

					if (parameterInfo != null)
					{
						parameterValue.UserValue = parameterInfo.Value;
					}
				}
			}
		}

		private void SaveQueryParameters(ConcreteTemplateNodeDefinition savedNode, ConnectionGroupInfo connection)
		{
			if (savedNode == null)
			{
				return;
			}

			var templateNodeInfo = savedNode.TemplateNode;

			Program.Model.GetVaultProcessor(connection)
				.CurrentStorage.Save(templateNodeInfo);
		}
		#endregion

		#region INotifyPropertyChanged
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;

			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion
	}
}
