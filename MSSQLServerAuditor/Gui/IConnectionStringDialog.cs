using System.Windows.Forms;

namespace MSSQLServerAuditor.Gui
{
	public interface IConnectionStringDialog
	{
		string ConnectionString { get; }
		string ConnectionName   { get; }
		bool   IsOdbc           { get; }

		DialogResult ShowDialog();
	}
}
