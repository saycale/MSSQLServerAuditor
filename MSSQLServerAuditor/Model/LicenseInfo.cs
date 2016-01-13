using System;
using System.Globalization;
using System.Xml.Serialization;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Model
{
	/// <summary>
	/// Info for instance license
	/// </summary>
	[Serializable]
	public class LicenseInfo
	{
		private string _hostName;
		private string _osUserName;
		private string _expiryDate;
		private string _signature;
		private string _buildexpirydate;

		public LicenseInfo()
		{
			this._hostName        = null;
			this._osUserName      = null;
			this._expiryDate      = null;
			this._signature       = null;
			this._buildexpirydate = null;
		}

		/// <summary>
		/// Host name of user PC
		/// </summary>
		[XmlAttribute(AttributeName = "hostname")]
		public string HostName
		{
			get { return this._hostName; }
			set { this._hostName = value; }
		}

		/// <summary>
		/// OS user name
		/// </summary>
		[XmlAttribute(AttributeName = "osusername")]
		public string OsUserName
		{
			get { return this._osUserName; }
			set { this._osUserName = value; }
		}

		/// <summary>
		/// License expiry date as string (yyyyMMdd)
		/// </summary>
		[XmlAttribute(AttributeName = "expirydate")]
		public string ExpiryDateStr
		{
			get { return this._expiryDate; }
			set { this._expiryDate = value; }
		}

		/// <summary>
		/// License expiry date as DateTime (end of the day)
		/// </summary>
		public DateTime ExpiryDate
		{
			get { return DateTime.ParseExact(ExpiryDateStr, "yyyyMMdd", CultureInfo.InvariantCulture).Date.AddSeconds(-1).AddDays(1); }
		}

		/// <summary>
		/// Programm expiry date as string (yyyyMMdd)
		/// </summary>
		[XmlAttribute(AttributeName = "buildexpirydate")]
		public string BuildexpirydateStr
		{
			get { return this._buildexpirydate; }
			set { this._buildexpirydate = value; }
		}

		/// <summary>
		/// Programm expiry date as DateTime (end of the day)
		/// </summary>
		public DateTime BuildExpiryDate
		{
			get
			{
				if (!string.IsNullOrEmpty(BuildexpirydateStr))
				{
					return
						DateTime.ParseExact(BuildexpirydateStr, "yyyyMMdd", CultureInfo.InvariantCulture)
							.AddSeconds(-1)
							.Date.AddDays(1);
				}

				return DateTime.MaxValue;
			}
		}

		/// <summary>
		/// License signature
		/// </summary>
		[XmlAttribute(AttributeName = "signature")]
		public string Signature
		{
			get { return this._signature; }
			set { this._signature = value; }
		}

		/// <summary>
		/// Check license correctness for available data fields (machine name, os user, expiry date)
		/// </summary>
		public LicenseState IsLicenseInfoCorrect()
		{
			var foundProblems = new LicenseState();

			if (Environment.MachineName.ToLower() != HostName.ToLower())
			{
				foundProblems.AddProblem(LicenseProblemType.WrongHost, "current:" + Environment.MachineName.ToLower() + "; licensed for:" + HostName.ToLower());
			}

			var currentUserName = Environment.UserDomainName.ToLower() + @"\" + Environment.UserName.ToLower();

			if (currentUserName != OsUserName.ToLower())
			{
				foundProblems.AddProblem(LicenseProblemType.WrongUser, "current:" + currentUserName + "; licensed for:" + OsUserName.ToLower());
			}

			return foundProblems;
		}
	}
}
