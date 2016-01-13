namespace MSSQLServerAuditor.SQLite.Common
{
    using System.Data.SQLite;
    using System.IO;

    public static class SQLiteConnectionExtensions
    {
        public static OpenCloseConnectionWrapper OpenWrapper(this SQLiteConnection connection)
        {
            return new OpenCloseConnectionWrapper(connection);
        }

        public static byte[] GetBytes(this SQLiteDataReader reader)
        {
            const int ChunkSize = 2 * 1024;
            var buffer = new byte[ChunkSize];
            var fieldOffset = 0L;

            using (var stream = new MemoryStream())
            {
                long bytesRead = 0L;

                while ((bytesRead = reader.GetBytes(0, fieldOffset, buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, (int)bytesRead);
                    fieldOffset += bytesRead;
                }
                return stream.ToArray();
            }
        }
    }
}