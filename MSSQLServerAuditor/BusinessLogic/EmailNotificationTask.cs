using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Scheduling;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.Preprocessor;

namespace MSSQLServerAuditor.BusinessLogic
{
	class EmailNotificationTask
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private List<EmailNotificationTask>    _allEmailTask;
		private Job                            _job;
		private VisualizeProcessor             _visualizeProcessor;
		private List<LinkedResource>           _linkedResources;
		private MailMessage                    _message;
		private string                         _messageHtml;
		private EmailNotificationSettings      _EmailNotificationSettings;

		public EmailNotificationTask(
			List<EmailNotificationTask> allEmailTask,
			Job                         job,
			VisualizeProcessor          visualizeProcessor
		)
		{
			this._allEmailTask              = allEmailTask;
			this._job                       = job;
			this._visualizeProcessor        = visualizeProcessor;
			this._linkedResources           = new List<LinkedResource>();
			this._messageHtml               = string.Empty;
			this._EmailNotificationSettings = Program.Model.Settings.EmailNotificationSettings;

			this._allEmailTask.Add(this);
		}

		public void onQueryResult(Task<NodeDataProvider> task)
		{
			if (this._EmailNotificationSettings == null)
			{
				log.ErrorFormat("job:'{0}'; EmailNotificationSettings node of UserSettings.xml is absent or empty", this._job);
				return;
			}

			NodeDataProvider dataProvider  = task.Result;
			Size             clientSize    = this._EmailNotificationSettings.chartSettings.ClientSize;
			float            floatOut      = 0.0F;
			float            dpiResolution = 92.0F; //default value

			if (!string.IsNullOrEmpty(this._EmailNotificationSettings.chartSettings.ImageResolution))
			{
				if (float.TryParse(this._EmailNotificationSettings.chartSettings.ImageResolution, out floatOut))
				{
					dpiResolution = floatOut;
				}
			}

			GraphicsInfo graphicsInfo = new GraphicsInfo(clientSize, dpiResolution);

			VisualizeData result = this._visualizeProcessor.GetVisualizeData(
				dataProvider,
				graphicsInfo
			);

			this._message = new MailMessage();

			if (result.PreprocessorAreas != null)
			{
				foreach (PreprocessorAreaData paData in result.PreprocessorAreas)
				{
					foreach (PreprocessorData pData in paData.Preprocessors)
					{
						IContentFactory factory = pData.ContentFactory;

						if (factory.CanCreateMailContent)
						{
							try
							{
								MailContent mailContent = factory.CreateMailContent();

								this._messageHtml = mailContent.Message;

								if (mailContent.Resource != null)
								{
									this._linkedResources.Add(mailContent.Resource);
								}
							}
							catch (Exception ex)
							{
								log.ErrorFormat("job:'{0}';Exception:'{1}'", this._job, ex);
								return;
							}
						}
					}
				}
			}

			this.sendMessage();
		}

		private void sendMessage()
		{
			SmtpClient client = new SmtpClient(
				this._EmailNotificationSettings.SmtpSettings.ServerAddress,
				this._EmailNotificationSettings.SmtpSettings.port
			);

			if (this._EmailNotificationSettings.SmtpSettings.AuthenticationRequired)
			{
				client.Credentials = new NetworkCredential(
					this._EmailNotificationSettings.SmtpSettings.UserName,
					this._EmailNotificationSettings.SmtpSettings.Password
				);

				client.EnableSsl = this._EmailNotificationSettings.SmtpSettings.SSL;
			}
			else
			{
				client.UseDefaultCredentials = true;
			}

			MailAddress from = new MailAddress(
				this._EmailNotificationSettings.SmtpSettings.SenderEmailAddress,
				"MSSQLServerAuditor notification",
				Encoding.UTF8
			);

			this._message.From = from;

			if (string.IsNullOrEmpty(this._EmailNotificationSettings.RecipientEmailAddress))
			{
				log.ErrorFormat("[{0}] Email address or user group name is not specified",
					this._job.Settings.Name
				);

				return;
			}

			string emailAddrListStr = this._EmailNotificationSettings.RecipientEmailAddress;

			this._message.To.Add(emailAddrListStr);

			this._message.BodyEncoding    = Encoding.UTF8;
			this._message.Subject         = "MSSQLServerAuditor";
			this._message.SubjectEncoding = Encoding.UTF8;

			AlternateView html_view = AlternateView.CreateAlternateViewFromString(this._messageHtml, null, "text/html");

			this._message.AlternateViews.Add(html_view);

			foreach (LinkedResource lr in this._linkedResources)
			{
				html_view.LinkedResources.Add(lr);
			}

			try
			{
				client.Send(this._message);
			}
			catch(Exception ex)
			{
				string errMess = ex.Message;

				if (ex.InnerException != null)
				{
					errMess += " " + ex.InnerException.Message;
				}

				log.ErrorFormat("[{0}] {1}", this._message, errMess);
			}

			this._allEmailTask.Remove(this);
		}
	}
}
