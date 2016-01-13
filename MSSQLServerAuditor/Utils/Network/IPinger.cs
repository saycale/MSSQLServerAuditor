using System;

namespace MSSQLServerAuditor.Utils.Network
{
	public interface IPinger
	{
		IAsyncResult BeginPing(AsyncCallback callback, object state);

		PingResult EndPing(IAsyncResult asyncResult);
	}
}
