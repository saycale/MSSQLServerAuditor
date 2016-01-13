using System.Drawing;

namespace MSSQLServerAuditor.Utils
{
	public static class Colors
	{
		public static Color FromString(string fontColor)
		{
			string hexPrefix = HexHelper.VerifyHex(fontColor.ToUpper())
				? "#"
				: string.Empty;

			return ColorTranslator.FromHtml(hexPrefix + fontColor);
		}
	}
}
