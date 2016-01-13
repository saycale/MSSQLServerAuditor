namespace MSSQLServerAuditor.Utils
{
	public static class ObjectExtensions
	{
		public static string ToSafeString(this object obj)
		{
			return ToStringOrDefaultWhenNull(obj, string.Empty);
		}

		public static string ToStringOrDefaultWhenNull(this object obj, string defaultString)
		{
			return obj == null ? defaultString : obj.ToString();
		}
	}
}
