using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using log4net;
using MSSQLServerAuditor.Entities;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Utils;
using MSSQLServerAuditor.Model.Settings;

namespace MSSQLServerAuditor.Model
{
	/// <summary>
	/// Info about connection data source
	/// </summary>
	[Serializable]
	public class InstanceInfo
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private ServerProperties   _serverProperties;
		private string             _instance;
		private string             _name;
		private LicenseState       _foundLicenseState;
		private SettingsInfo       _settingsInfo;

		/// <summary>
		/// Default constructor
		/// </summary>
		public InstanceInfo()
		{
			this._serverProperties    = null;
			this.IsEnabled            = true;
			this.IsODBC               = false;
			this.IsDynamicConnection  = false;
		}

		public InstanceInfo(bool isDynamicConnection) : this()
		{
			this.IsDynamicConnection = isDynamicConnection;
		}

		public InstanceInfo(ServerProperties serverProps) : this()
		{
			this._serverProperties = serverProps;
		}

		/// <summary>
		/// Name (text representation)
		/// </summary>
		[XmlAttribute(AttributeName = "name")]
		public string Name
		{
			get { return this._name; }

			set
			{
				this._name = value;

				// log.InfoFormat(
				//    @"InstanceInfo:ConnectionName:{0}",
				//    this._name
				// );
			}
		}

		/// <summary>
		/// Instance definition
		/// </summary>
		[XmlAttribute(AttributeName = "instance")]
		public string Instance
		{
			get { return this._instance; }

			set
			{
				this._instance = value;

				// log.InfoFormat(
				//    @"InstanceInfo:InstanceName:'{0}'",
				//    this._instance
				// );
			}
		}

		[XmlAttribute(AttributeName = "isDynamicConnection")]
		public bool IsDynamicConnection { get; set; }

		/// <summary>
		/// Db type.
		/// </summary>
		[XmlAttribute(AttributeName = "type")]
		public string DbType { get; set; }

		/// <summary>
		/// Is it 'odbc' data source type?
		/// </summary>
		[XmlAttribute(AttributeName = "odbc")]
		public bool IsODBC { get; set; }

		[XmlAttribute(AttributeName = "innerOdbcConnection")]
		public string InnerOdbcConnectionString { get; set; }

		public QuerySource Type
		{
			get
			{
				QuerySource source;

				if (!Enum.TryParse(this.DbType, true, out source))
				{
					log.ErrorFormat("Unknown server type: {0}", this.DbType);
				}

				return source;
			}
		}

		/// <summary>
		/// Authentication
		/// </summary>
		[XmlElement(ElementName = "authentication")]
		public AuthenticationInfo Authentication { get; set; }

		/// <summary>
		/// License info
		/// </summary>
		[XmlElement(ElementName = "license")]
		public LicenseInfo LicenseInfo { get; set; }

		/// <summary>
		/// Is instance enabled for querying
		/// </summary>
		[XmlIgnore]
		public bool IsEnabled { get; set; }

		/// <summary>
		/// Reference to parent ConnectionGroup
		/// </summary>
		[XmlIgnore]
		public ConnectionGroupInfo ConnectionGroup { get; set; }

		/// <summary>
		/// Get hash.
		/// </summary>
		/// <returns>Hash.</returns>
		public string GetHash()
		{
			return Instance + ";" + Authentication.Username + ";" +
				Authentication.IsWindows +
				";" + this.LicenseInfo.HostName.ToLower() + ";" + this.LicenseInfo.OsUserName.ToLower() + ";" +
				this.LicenseInfo.ExpiryDateStr;
		}

		/// <summary>
		/// The validate license.
		/// </summary>
		/// <param name="fullSlowCheck">The full slow check.</param>
		/// <returns>
		/// The <see cref="LicenseState" />.
		/// </returns>
		public LicenseState ValidateLicense(bool fullSlowCheck, bool withCheckConnection = true)
		{
			if (this._foundLicenseState != null)
			{
				return this._foundLicenseState;
			}

			this._foundLicenseState = new LicenseState
			{
				Instance = this.Instance
			};

			if (this.LicenseInfo == null)
			{
				if (AppVersionHelper.IsNotRelease())
				{
					return this._foundLicenseState;
				}

				this._foundLicenseState.AddProblem(LicenseProblemType.LicenseNotDefined, string.Empty);
			}
			else
			{
				this._foundLicenseState.AddProblems(this.LicenseInfo.IsLicenseInfoCorrect());

				DateTime dateTime;

				if (fullSlowCheck && withCheckConnection)
				{
					if (this._serverProperties != null)
					{
						dateTime = this._serverProperties.Date;
					}
					else
					{
						try
						{
							ServerProperties props = ServerProperties.Query(this);

							dateTime = props.Date;

							this._serverProperties = props;
						}
						catch (Exception exc)
						{
							log.Error("Exception:", exc);

							log.ErrorFormat(
								"Instance:'{0}';Authentication:'{1}';Exception:'{2}'",
								Instance,
								Authentication,
								exc
							);

							dateTime = DateTime.MaxValue;
						}
					}
				}
				else
				{
					dateTime = DateTime.Now;
				}

				var buildDateObject =
					Assembly.GetExecutingAssembly()
						.GetCustomAttributes(typeof (BuildDateAttribute), false)
						.FirstOrDefault();

				DateTime buildDate = DateTime.MaxValue;

				if (buildDateObject is BuildDateAttribute)
				{
					buildDate = (buildDateObject as BuildDateAttribute).BuildDate;
				}

				if (dateTime == DateTime.MaxValue)
				{
					this._foundLicenseState.AddProblem(LicenseProblemType.CantConnect, string.Empty);
				}
				else if (this._foundLicenseState.IsCorrect && fullSlowCheck && (dateTime > this.LicenseInfo.ExpiryDate))
				{
					this._foundLicenseState.AddProblem(LicenseProblemType.Expired, string.Empty);
				}
				else if (buildDate > this.LicenseInfo.BuildExpiryDate)
				{
					this._foundLicenseState.AddProblem(LicenseProblemType.BuildExpiryDateNotValid, string.Empty);
				}
				else
				{
					if (AppVersionHelper.IsNotDebug())
					{
						string hash = this.GetHash();

						CryptoProcessor cryptoProcessor =
							new CryptoProcessor(
								Program.Model.Settings.SystemSettings.PublicKeyXmlSign,
								Program.Model.Settings.SystemSettings.PrivateKeyXmlDecrypt
							);

						if (!cryptoProcessor.Verify(hash, this.LicenseInfo.Signature))
						{
							this._foundLicenseState.AddProblem(LicenseProblemType.WrongSignature, string.Empty);
						}
					}
				}
			}

			var result = this._foundLicenseState;

			if (!fullSlowCheck)
			{
				this._foundLicenseState = null;
			}

			return result;
		}

		/// <summary>
		/// Text representation of license expiry date
		/// </summary>
		public string LicenseExpiryDate
		{
			get { return (this.LicenseInfo != null) ? this.LicenseInfo.ExpiryDate.ToShortDateString() : "-"; }
		}

		/// <summary>
		/// Text representation of build expiry date
		/// </summary>
		public string Buildexpirydate
		{
			get { return (this.LicenseInfo != null) ? this.LicenseInfo.BuildExpiryDate.ToShortDateString() : "-"; }
		}

		public override string ToString()
		{
			return GetDisplayString();
		}

		public void SetSettings(SettingsInfo settings)
		{
			this._settingsInfo = settings;
		}

		public string GetDisplayString()
		{
			switch (this.Type)
			{
				case QuerySource.SQLite:
				case QuerySource.ActiveDirectory:
				case QuerySource.EventLog:
				case QuerySource.NetworkInformation:
					return Instance;
			}

			Dictionary<string, string> props = new Dictionary<string, string>();

			if (this.IsODBC)
			{
				if (!string.IsNullOrEmpty(InnerOdbcConnectionString))
				{
					return InnerOdbcConnectionString;
				}

				props.Add("Dsn", Instance);
				props.Add("uid", Authentication.Username);
				props.Add("pwd", Authentication.Password);
			}
			else
			{
				switch (this.Type)
				{
					case QuerySource.MSSQL:
						props.Add("Server", Instance);

						if (Authentication.IsWindows)
						{
							props.Add("Integrated Security", "True");
							props.Add("Pooling",             "False");
						}
						else
						{
							props.Add("User Id",  Authentication.Username);
							props.Add("Password", Authentication.Password);
						}

						break;

					case QuerySource.TDSQL:
						props.Add("Data Source", Instance);
						props.Add("User Id",     Authentication.Username);
						props.Add("Password",    Authentication.Password);

						break;

					case QuerySource.SQLiteExternal:
						props.Add("Data Source", Instance);
						break;

					default:
						log.ErrorFormat(
							"Unknown server type: {0}",
							this.Type
						);

						return string.Empty;
				}
			}

			return string.Join(";", props.Select(
				kvp => string.Format("{0}={1}", kvp.Key, kvp.Value)
			));
		}

		public string GetConnectionString()
		{
			Dictionary<string, string> props      = new Dictionary<string, string>();
			int                        sqlTimeout = 0;

			switch (this.Type)
			{
				case QuerySource.SQLite:
				case QuerySource.ActiveDirectory:
				case QuerySource.EventLog:
				case QuerySource.NetworkInformation:
					return Instance;
			}

			if (Program.Model != null)
			{
				sqlTimeout = Program.Model.Settings.SqlTimeout;
			}
			else
			{
				sqlTimeout = this._settingsInfo.SqlTimeout;
			}

			if (this.IsODBC)
			{
				if (!string.IsNullOrEmpty(InnerOdbcConnectionString))
				{
					return InnerOdbcConnectionString;
				}

				props.Add("Dsn", Instance);
				props.Add("uid", Authentication.Username);
				props.Add("pwd", Authentication.Password);
			}
			else
			{
				switch (this.Type)
				{
					case QuerySource.MSSQL:
						props.Add("Server",                  Instance);
						props.Add("Application Name",        Application.ProductName + " " + Application.ProductVersion);
						props.Add("Asynchronous Processing", "True");

						if (Authentication.IsWindows)
						{
							props.Add("Integrated Security", "True");
							props.Add("Pooling",             "False");
						}
						else
						{
							props.Add("User Id",  Authentication.Username);
							props.Add("Password", Authentication.Password);
						}

						props.Add("Connection Timeout", Convert.ToString(sqlTimeout));
						break;

					case QuerySource.TDSQL:
						props.Add("Data Source",        Instance);
						props.Add("User Id",            Authentication.Username);
						props.Add("Password",           Authentication.Password);
						props.Add("Connection Timeout", Convert.ToString(sqlTimeout));
						break;

					case QuerySource.SQLiteExternal:
						props.Add("Data Source",          Instance);
						props.Add("PRAGMA locking_mode",  "NORMAL");
						props.Add("PRAGMA journal_mode",  "WAL");
						props.Add("PRAGMA synchronous",   "off");
						props.Add("PRAGMA count_changes", "off");
						props.Add("PRAGMA temp_store",    "2");
						props.Add("PRAGMA encoding",      "\"UTF-8\"");
						props.Add("PRAGMA query_only",    "True");
						props.Add("mode",                 "ro");
						break;

					default:
						log.ErrorFormat(
							"Unknown server type: {0}",
							this.Type
						);

						return string.Empty;
				}
			}

			return string.Join(";", props.Select(
				kvp => string.Format("{0}={1}", kvp.Key, kvp.Value)
			));
		}

		/// <summary>
		/// Returns server properties
		/// </summary>
		public ServerProperties ServerProperties
		{
			get { return this._serverProperties; }
		}

		public ServerProperties GetServerPropertiesSafe()
		{
			if (this._serverProperties == null)
			{
				return new ServerProperties(
					new InstanceVersion(),
					Name,
					DateTime.MaxValue
				);
			}

			return this._serverProperties;
		}

		public ServerProperties InitServerProperties(CurrentStorage storage, int timeout = 0)
		{
			if (this._serverProperties == null)
			{
				try
				{
					this._serverProperties = ServerProperties.Query(this, timeout);

					storage.ServerInstanceDirectory.GetId(this.ConnectionGroup, this); // Save properties
				}
				catch (Exception exc)
				{
					log.WarnFormat(
						"Unable to retrieve instance version from remote server. Instance:'{0}';Authentication:'{1}';Exception:'{2}'",
						Instance,
						Authentication,
						exc
					);

					ServerProperties storedProps = LoadServerProperties(storage);

					if (storedProps == null)
					{
						log.WarnFormat(
							"Instance version is not available. Instance:'{0}';Authentication:'{1}';Exception:'{2}'",
							Instance,
							Authentication,
							exc
						);

						throw;
					}
				}
			}

			return this._serverProperties;
		}

		public ServerProperties LoadServerProperties(CurrentStorage storage)
		{
			ServerProperties storedProps = ServerProperties.Load(this, storage);

			if (storedProps != null)
			{
				this._serverProperties = storedProps;
			}

			return this._serverProperties;
		}

		public void SetServerProperties(ServerProperties props)
		{
			this._serverProperties = props;
		}
	}
}
