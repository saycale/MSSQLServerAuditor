using MSSQLServerAuditor.BusinessLogic;

namespace MSSQLServerAuditor.Preprocessor
{
	public enum VerticalTextAlign
	{
		Top,
		Bottom,
	}

	public enum TextAlign
	{
		Left,
		Right,
		Center,
	}

	/// <summary>
	/// Interface for XSL preprocessor
	/// </summary>
	public interface IPreprocessor
	{
		IContentFactory CreateContentFactory(string id, string configuration);

		GraphicsInfo     GraphicsInfo { get; }
		NodeDataProvider DataProvider { get; }
	}
}
