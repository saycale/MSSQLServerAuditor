namespace MSSQLServerAuditor.SQLite.Commands
{
    using System.Data.SQLite;
    using System.Reflection;

    using log4net;

    public class SqlScalarCommand : CommandBase
    {
        private static readonly ILog       Log        = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SQLiteParameter[] parameters = null;
        private readonly string            sql        = null;

        public SqlScalarCommand(SQLiteConnection connection, string sql, params SQLiteParameter[] parameters)
            : base(connection)
        {
            this.sql        = sql;
            this.parameters = parameters;
        }

        protected override ILog Logger
        {
            get
            {
                return Log;
            }
        }

        public object Result { get; private set; }

        protected override long InternalExecute()
        {
            this.Result = this.ExecuteScalar(this.sql, this.parameters);

			return 0L;
        }
    }
}