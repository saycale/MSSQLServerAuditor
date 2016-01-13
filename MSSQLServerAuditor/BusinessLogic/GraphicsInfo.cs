using System.Drawing;

namespace MSSQLServerAuditor.BusinessLogic
{
	public class GraphicsInfo
	{
		public GraphicsInfo(Size size, float dpiX, float dpiY)
		{
			this.Size = size;
			this.DpiX = dpiX;
			this.DpiY = dpiY;
		}

		public GraphicsInfo(Size size, float dpi) : this(size, dpi, dpi)
		{
		}

		public float DpiX { get; private set; }
		public float DpiY { get; private set; }
		public Size  Size { get; private set; }
	}
}