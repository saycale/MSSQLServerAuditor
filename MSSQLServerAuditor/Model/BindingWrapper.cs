using System;

namespace MSSQLServerAuditor.Model
{
	public class BindingWrapper<T> where T : class
	{
		public T Item               { get; private set; }
		public string DisplayMember { get; private set; }

		public BindingWrapper(T item, Func<T, string> displayFactory)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}

			if (displayFactory == null)
			{
				throw new ArgumentNullException("displayFactory");
			}

			this.Item          = item;
			this.DisplayMember = displayFactory(item);
		}

		public override string ToString()
		{
			return this.DisplayMember;
		}
	}
}
