using System.Reflection;
using log4net;
using MSSQLServerAuditor.Model;

namespace MSSQLServerAuditor.BusinessLogic.LocalStorage
{
	public class MultiQueryResultReader
	{
		private static readonly ILog            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly      MsSqlAuditorModel _msSqlAuditor;
		private readonly      StorageManager    _storageManager;

		public MultiQueryResultReader()
		{
			this._msSqlAuditor   = null;
			this._storageManager = null;
		}

		public MultiQueryResultReader(
			MsSqlAuditorModel msSqlAuditor,
			StorageManager    storageManager
		) : this()
		{
			this._msSqlAuditor   = msSqlAuditor;
			this._storageManager = storageManager;
		}

		public MultyQueryResultInfo Read(
			ConnectionGroupInfo            connectionGroup,
			ConcreteTemplateNodeDefinition concreteTemplateNode
		)
		{
			var result = new MultyQueryResultInfo();

			this.ReadFromMeta(connectionGroup, concreteTemplateNode, result);
			this.ReadFromCodeGuard(concreteTemplateNode, result);

			return result;
		}

		private void ReadFromMeta(
			ConnectionGroupInfo            connectionGroup,
			ConcreteTemplateNodeDefinition concreteTemplateNode,
			MultyQueryResultInfo           result
		)
		{
			NodeResultReader nodeResultReader = new MetaResultReader(
				this._msSqlAuditor,
				this._storageManager,
				connectionGroup,
				concreteTemplateNode
			);

			nodeResultReader.ReadTo(result);
		}

		private void ReadFromCodeGuard(
			ConcreteTemplateNodeDefinition concreteTemplateNode,
			MultyQueryResultInfo           result
		)
		{
			NodeResultReader nodeResultReader = new CodeGuardResultReader(
				this._msSqlAuditor,
				this._storageManager,
				concreteTemplateNode
			);

			nodeResultReader.ReadTo(result);
		}
	}
}
