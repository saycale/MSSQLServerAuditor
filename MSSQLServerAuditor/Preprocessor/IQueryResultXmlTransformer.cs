using System.Xml;

namespace MSSQLServerAuditor.Preprocessor
{
	public interface IQueryResultXmlTransformer
	{
		XmlDocument Transform(QueryResultDataSource dataSource);
	}
}
