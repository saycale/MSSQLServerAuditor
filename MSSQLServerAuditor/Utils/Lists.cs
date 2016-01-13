using System.Collections.Generic;

namespace MSSQLServerAuditor.Utils
{
	public static class Lists
	{
		public static List<T> Of<T>(T firstItem, params T[] otherItems)
		{
			List<T> list = new List<T> { firstItem };

			if (otherItems != null)
			{
				list.AddRange(otherItems);
			}

			return list;
		}
	}
}
