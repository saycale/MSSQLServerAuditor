using System.Net.Mail;

namespace MSSQLServerAuditor.Preprocessor
{
	public class MailContent
	{
		public string         Message  { get; set; }
		public LinkedResource Resource { get; set; }
	}
}
