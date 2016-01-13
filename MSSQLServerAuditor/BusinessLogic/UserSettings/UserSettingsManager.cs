using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables.UserSettings;

namespace MSSQLServerAuditor.BusinessLogic.UserSettings
{
	public class UserSettingsManager
	{
		private readonly string         _language;
		private readonly CurrentStorage _currentStorage;

		public UserSettingsManager()
		{
			this._currentStorage = Program.Model.DefaultVaultProcessor.CurrentStorage;
		}

		public UserSettingsManager(string language) : this()
		{
			this._language = language;
		}

		public void SaveUserSettings(UserSettingsRow userSettings)
		{
			userSettings.Language = this._language;

			this._currentStorage.UserSettingsTable.InsertOrUpdateRowForSure(userSettings);
		}

		public UserSettingsRow LoadUserSettings(long templateNodeId)
		{
			return this._currentStorage.UserSettingsTable
				.GetByTemplateNodeAndLanguage(templateNodeId, this._language);
		}
	}
}
