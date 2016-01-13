using System.Collections.Generic;
using System.ComponentModel;

namespace MSSQLServerAuditor.Model.UserSettingsParameters
{
	public interface IQueryParameters
	{
		/// <summary>
		/// Do long and dangerous operation
		/// </summary>
		void Init();

		/// <summary>
		/// List queries of node
		/// </summary>
		BindingList<QueryKey> Queries { get; set; }

		/// <summary>
		/// List parameters of selected query
		/// </summary>
		BindingList<ParameterInfoLocalizable> Parameters { get; set; }

		/// <summary>
		/// Add editable parameter
		/// </summary>
		void AddParameter();

		/// <summary>
		/// Try remove editable permeters from selected
		/// </summary>
		void RemoveSelectedParameters();

		/// <summary>
		/// Set Selected Parametes
		/// </summary>
		void SetSelectedParametes(IEnumerable<ParameterInfoLocalizable> parameters);

		/// <summary>
		/// Set or Get selected query
		/// </summary>
		QueryKey SelectedQuery { get; set; }

		/// <summary>
		/// Name node
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Apply changed
		/// </summary>
		void ApplyChanged();

		bool AvailbleEdit { get; }

		bool AvailableRemove { get; }

		void TryUpdate();

		bool IsHideTypeColumn { get; }

		bool IsHideTabs { get; }
	}
}
