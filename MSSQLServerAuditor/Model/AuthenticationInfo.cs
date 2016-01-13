using System;
using System.Xml.Serialization;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Model
{
	/// <summary>
	/// Authentication info
	/// </summary>
	[Serializable]
	public struct AuthenticationInfo
	{
		private bool   _isWindows;
		private string _username;
		private string _password;

		/// <summary>
		/// Use Windows authentication
		/// </summary>
		[XmlAttribute(AttributeName = "isWindows")]
		public bool IsWindows
		{
			get { return this._isWindows; }
			set { this._isWindows = value; }
		}

		/// <summary>
		/// User name
		/// </summary>
		[XmlAttribute(AttributeName = "username")]
		public string Username
		{
			get { return this._username; }
			set { this._username = value; }
		}

		/// <summary>
		/// User password
		/// </summary>
		[XmlAttribute(AttributeName = "password")]
		public string Password
		{
			get { return this._password; }
			set { this._password = value; }
		}

		/// <summary>
		/// Object to string.
		/// </summary>
		/// <returns>String objects.</returns>
		public override string ToString()
		{
			if (IsWindows)
			{
				return "[Windows]";
			}

			return this.Username;
		}

		/// <summary>
		/// Return current login from Username field or Windows account
		/// </summary>
		/// <returns></returns>
		public string GetCurrentLogin()
		{
			if (this.IsWindows)
			{
				return Environment.UserDomainName + @"\" + Environment.UserName;
			}
			else
			{
				return this.Username;
			}
		}
	}
}