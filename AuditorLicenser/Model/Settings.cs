using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Licenser.Model
{
	[Serializable]
	public class Settings
	{
		private const string SystemFileName = "MSSQLServerAuditorLicenser.SystemSettings.xml";
		private const string UserFileName   = "MSSQLServerAuditorLicenser.UserSettings.xml";

		private static string GetUserOnesFileName()
		{
			var userDocsAppFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MSSQLServerAuditorLicenser");

			return Path.Combine(userDocsAppFolder, UserFileName);
		}

		public static Settings SystemOnes { get; private set; }

		public static Settings UserOnes { get; private set; }

		public static void SeveUserOnes()
		{
			var userFile = GetUserOnesFileName();
			var path     = Path.GetDirectoryName(userFile);

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			UserOnes.SaveTo(userFile);
		}

		static Settings()
		{
			string sysFile = Path.Combine(Application.StartupPath, SystemFileName);
			SystemOnes = new Settings() { UiLanguage = "en" };

			if (File.Exists(sysFile))
			{
				SystemOnes = LoadFromXml(sysFile);
			}

			string userFile = GetUserOnesFileName();
			UserOnes = (Settings)SystemOnes.MemberwiseClone();

			if (File.Exists(userFile))
			{
				UserOnes = LoadFromXml(userFile);
			}
		}

		private static Settings LoadFromXml(string fileName)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Settings));

			using (StreamReader reader = new StreamReader(fileName))
			{
				return (Settings)serializer.Deserialize(reader);
			}
		}

		private void SaveTo(string fileName)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Settings));

			using (StreamWriter writer = new StreamWriter(fileName))
			{
				serializer.Serialize(writer, this);
			}
		}

		[XmlElement("interface_lang")]
		public string UiLanguage { get; set; }

		/// <summary>
		/// Available UI language.
		/// </summary>
		[XmlElement("available_ui_lang")]
		public List<string> AvailableUiLanguages { get; set; }

		public static event EventHandler UserOnesChanged;

		public Settings Clone()
		{
			return (Settings)MemberwiseClone();
		}

		public static void AssignUserOnes(Settings settings)
		{
			UserOnes = settings;

			SeveUserOnes();

			if (UserOnesChanged != null)
			{
				UserOnesChanged(UserOnes, EventArgs.Empty);
			}
		}
	}
}
