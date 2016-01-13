using MSSQLServerAuditor.BusinessLogic;

namespace MSSQLServerAuditor.Preprocessor
{
	public abstract class Preprocessor : IPreprocessor
	{
		protected readonly NodeDataProvider _dataProvider;
		protected readonly GraphicsInfo     _graphicsInfo;

		protected Preprocessor(
			NodeDataProvider dataProvider,
			GraphicsInfo     graphicsInfo
		)
		{
			this._dataProvider = dataProvider;
			this._graphicsInfo = graphicsInfo;
		}

		public abstract IContentFactory CreateContentFactory(string id, string configuration);

		public GraphicsInfo GraphicsInfo
		{
			get { return this._graphicsInfo; }
		}

		public NodeDataProvider DataProvider
		{
			get { return this._dataProvider; }
		}
	}
}
