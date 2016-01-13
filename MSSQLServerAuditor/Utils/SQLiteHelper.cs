using System.CodeDom;
using System.Data.SQLite;

namespace MSSQLServerAuditor.Utils
{
    using MSSQLServerAuditor.SQLite.Commands;

    internal class SQLiteHelper
    {
        public const string CurrentTimestamp = "(datetime('now', 'localtime'))";

        internal static SQLiteParameter GetParameter(string name, object value)
        {
            SQLiteParameter result = new SQLiteParameter(name, value.GetType().ToSqlDbType().ToDbType());
            result.Value = value;
            return result;
        }
    }
}
