using MSSQLServerAuditor.BusinessLogic.LocalStorage;
using MSSQLServerAuditor.Model;

namespace MSSQLServerAuditor.Preprocessor
{
	public class QueryResultDataSource
	{
		private readonly ConcreteTemplateNodeDefinition _nodeDefinition;
		private readonly MsSqlAuditorModel              _model;
		private readonly MultyQueryResultInfo           _queriesResult;

		public QueryResultDataSource(
			MsSqlAuditorModel              model,
			ConcreteTemplateNodeDefinition nodeDefinition
		)
		{
			this._model          = model;
			this._nodeDefinition = nodeDefinition;
			this._queriesResult  = ReadQueriesResult();
		}

		public MsSqlAuditorModel Model
		{
			get { return this._model; }
		}

		public ConcreteTemplateNodeDefinition NodeDefinition
		{
			get { return this._nodeDefinition; }
		}

		public MultyQueryResultInfo QueriesResult
		{
			get { return this._queriesResult; }
		}

		private MultyQueryResultInfo ReadQueriesResult()
		{
			ConnectionGroupInfo connectionGroup = this._nodeDefinition.Connection;
			IStorageManager     vaultProcessor  = this._model.GetVaultProcessor(connectionGroup);

			return vaultProcessor.ReadCurrentResult(connectionGroup, this._nodeDefinition);
		}
	}
}
