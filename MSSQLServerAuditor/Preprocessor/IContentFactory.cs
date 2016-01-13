using System.Windows.Forms;

namespace MSSQLServerAuditor.Preprocessor
{
	public interface IContentFactory
	{
		Control CreateControl();

		bool CanCreateMailContent { get; }

		MailContent CreateMailContent();

		IPreprocessor Preprocessor { get; }
	}
}
