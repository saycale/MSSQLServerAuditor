using System.Xml;
using MSSQLServerAuditor.Model;

namespace MSSQLServerAuditor.Preprocessor
{
	public class NodeDataProvider
	{
		private readonly QueryResultDataSource          _dataSource;
		private readonly ConcreteTemplateNodeDefinition _nodeDefinition;

		public NodeDataProvider(
			MsSqlAuditorModel              model,
			ConcreteTemplateNodeDefinition nodeDefinition
		)
		{
			this._nodeDefinition = nodeDefinition;
			this._dataSource     = new QueryResultDataSource(model, nodeDefinition);

			IQueryResultXmlTransformer transformer = new QueryResultXmlTransformer();

			XmlDocument = transformer.Transform(this._dataSource);
		}

		public XmlDocument XmlDocument { get; private set; }

		public ConcreteTemplateNodeDefinition NodeDefinition
		{
			get { return this._nodeDefinition; }
		}

		public MultyQueryResultInfo QueryResult
		{
			get { return this._dataSource.QueriesResult; }
		}
	}
}
