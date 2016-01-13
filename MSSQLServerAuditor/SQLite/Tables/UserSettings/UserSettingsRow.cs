using MSSQLServerAuditor.SQLite.Common;

namespace MSSQLServerAuditor.SQLite.Tables.UserSettings
{    
	public class UserSettingsRow : AutoincrementTableRow
    {
        public UserSettingsRow() : base(UserSettingsTable.CreateTableDefinition())
        {
        }

        public string Language
        {
            get
            {
                return this.GetValue(UserSettingsTable.Language, string.Empty);
            }
            set
            {
                this.SetValue(UserSettingsTable.Language, value);
            }
        }

        public long TemplateNodeId
        {
            get
            {
                return this.GetValue<long>(UserSettingsTable.TemplateNodeId);
            }
            set
            {
                this.SetValue(UserSettingsTable.TemplateNodeId, value);
            }
        }

        public string NodeUIName
        {
            get
            {
                return this.GetValue(UserSettingsTable.NodeUName, string.Empty);
            }
            set
            {
                this.SetValue(UserSettingsTable.NodeUName, value);
            }
        }

        public string NodeUIIcon
        {
            get
            {
                return this.GetValue(UserSettingsTable.NodeUIcon, string.Empty);
            }
            set
            {
                this.SetValue(UserSettingsTable.NodeUIcon, value);
            }
        }

        public bool NodeEnabled
        {
            get
            {
                return this.GetValue(UserSettingsTable.NodeEnabled, true);
            }
            set
            {
                this.SetValue(UserSettingsTable.NodeEnabled, value);
            }
        }

        public string NodeFontStyle
        {
            get
            {
                return this.GetValue(UserSettingsTable.NodeFontStyle, string.Empty);
            }
            set
            {
                this.SetValue(UserSettingsTable.NodeFontStyle, value);
            }
        }

        public string NodeFontColor
        {
            get
            {
                return this.GetValue(UserSettingsTable.NodeFontColor, string.Empty);
            }
            set
            {
                this.SetValue(UserSettingsTable.NodeFontColor, value);
            }
        }

    }
}