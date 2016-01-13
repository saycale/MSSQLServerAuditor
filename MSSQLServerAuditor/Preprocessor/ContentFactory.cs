using System;
using System.Windows.Forms;

namespace MSSQLServerAuditor.Preprocessor
{
	public abstract class ContentFactory : IContentFactory
	{
		protected readonly string _id;
		protected readonly string _configuration;

		protected ContentFactory(
			IPreprocessor preprocessor,
			string        id,
			string        configuration
		)
		{
			this.Preprocessor   = preprocessor;
			this._id            = id;
			this._configuration = configuration;
		}

		public IPreprocessor Preprocessor
		{
			get; private set;
		}

		public abstract Control CreateControl();

		public virtual bool CanCreateMailContent
		{
			get { return false; }
		}

		public virtual MailContent CreateMailContent()
		{
			throw new NotSupportedException();
		}
	}
}
