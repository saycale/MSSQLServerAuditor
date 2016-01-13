using System.Linq;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Groups;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.BusinessLogic.LocalStorage
{
	public abstract class NodeResultReader
	{
		private readonly MsSqlAuditorModel _msSqlAuditor;
		private readonly StorageManager    _storageManager;

		protected GroupDefinition  GroupDefinition { get; private set; }
		protected TemplateNodeInfo TemplateNode    { get; private set; }
		protected InstanceTemplate Settings        { get; private set; }

		protected NodeResultReader()
		{
			this._msSqlAuditor   = null;
			this._storageManager = null;

			this.GroupDefinition = null;
			this.TemplateNode    = null;
			this.Settings        = null;
		}

		protected NodeResultReader(
			MsSqlAuditorModel              msSqlAuditor,
			StorageManager                 storageManager,
			ConcreteTemplateNodeDefinition concreteTemplateNode
		) : this()
		{
			this._msSqlAuditor   = msSqlAuditor;
			this._storageManager = storageManager;

			if (concreteTemplateNode != null)
			{
				this.GroupDefinition = concreteTemplateNode.Group;
				this.TemplateNode    = concreteTemplateNode.TemplateNode;

				if (msSqlAuditor != null && msSqlAuditor.TemplateSettings != null && concreteTemplateNode.Connection != null)
				{
					this.Settings = msSqlAuditor.TemplateSettings.UserSettings.FirstOrDefault(
						i => i.TemplateName == concreteTemplateNode.Connection.TemplateFileName
							&& i.Connection.ParentKey == TemplateNode.IdsHierarchy
					);
				}
			}
		}

		protected CurrentStorage Storage
		{
			get { return this._storageManager.CurrentStorage; }
		}

		protected ReportStorage ReportStorage
		{
			get { return this._storageManager.ReportStorage; }
		}

		protected StorageManager StorageManager
		{
			get { return this._storageManager; }
		}

		protected MsSqlAuditorModel MsSqlAuditor
		{
			get { return this._msSqlAuditor; }
		}

		public abstract void ReadTo(MultyQueryResultInfo result);
	}
}
