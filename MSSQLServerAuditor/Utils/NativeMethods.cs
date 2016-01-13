using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;

namespace MSSQLServerAuditor.Utils
{
	// ReSharper disable InconsistentNaming
	internal static class ExternDll
	{
		public const string Gdi32 = "gdi32.dll";
		public const string Gdiplus = "gdiplus.dll";
		public const string Kernel32 = "kernel32.dll";
		public const string Ntdll = "ntdll.dll";
		public const string User32 = "user32.dll";
		public const string Uxtheme = "uxtheme.dll";
		public const string Psapi = "psapi.dll";
	}

	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNativeMethods
	{
		[DllImport(ExternDll.User32, ExactSpelling = true, EntryPoint = "GetDCEx", CharSet = CharSet.Auto)]
		private static extern IntPtr IntGetDCEx(HandleRef hWnd, HandleRef hrgnClip, int flags);

		public static IntPtr GetDCEx(HandleRef hWnd, HandleRef hrgnClip, int flags)
		{
			return IntGetDCEx(hWnd, hrgnClip, flags);
		}

		[DllImport(ExternDll.User32, ExactSpelling = true, EntryPoint = "ReleaseDC", CharSet = CharSet.Auto)]
		private static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

		public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
		{
			return IntReleaseDC(hWnd, hDC);
		}
	}

	[SuppressUnmanagedCodeSecurity]
	internal static class SafeNativeMethods
	{
		[DllImport(ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, EntryPoint = "CreateBitmap", CharSet = CharSet.Auto)]
		private static extern IntPtr /*HBITMAP*/ IntCreateBitmapShort(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, short[] lpvBits);

		public static IntPtr /*HBITMAP*/ CreateBitmap(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, short[] lpvBits)
		{
			return IntCreateBitmapShort(nWidth, nHeight, nPlanes, nBitsPerPixel, lpvBits);
		}

		[DllImport(ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
		public static extern IntPtr SelectObject(HandleRef hDC, HandleRef hObject);

		[DllImport(ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, EntryPoint = "CreateBrushIndirect", CharSet = CharSet.Auto)]
		private static extern IntPtr IntCreateBrushIndirect(NativeMethods.LOGBRUSH lb);

		public static IntPtr CreateBrushIndirect(NativeMethods.LOGBRUSH lb)
		{
			return IntCreateBrushIndirect(lb);
		}

		[DllImport(ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, EntryPoint = "DeleteObject", CharSet = CharSet.Auto)]
		internal static extern bool IntDeleteObject(HandleRef hObject);

		public static bool DeleteObject(HandleRef hObject)
		{
			return IntDeleteObject(hObject);
		}

		[DllImport(ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
		public static extern bool PatBlt(HandleRef hdc, int left, int top, int width, int height, int rop);

		[DllImport(ExternDll.Psapi, ExactSpelling = true)]
		public static extern bool EmptyWorkingSet(IntPtr hProcess);
	}

	[SuppressUnmanagedCodeSecurity]
	internal static class NativeMethods
	{
		public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

		public const int DCX_CACHE = 0x00000002;
		public const int DCX_LOCKWINDOWUPDATE = 0x00000400;
		public const int PATINVERT = 0x005A0049;
		public const int BS_PATTERN = 3;

		public const int WM_KEYFIRST = 0x0100;
		public const int WM_KEYLAST = 0x0108;
		public const int WM_KEYDOWN = 0x0100;
		public const int WM_SYSKEYDOWN = 0x0104;

		[StructLayout(LayoutKind.Sequential)]
		public class LOGBRUSH
		{
			public int lbStyle;
			public int lbColor;
			public IntPtr lbHatch;
		}
	}

	[SuppressUnmanagedCodeSecurity]
	internal static class ControlPaint
	{
		internal static IntPtr CreateHalftoneHBRUSH()
		{
			var grayPattern = new short[8];

			for (var i = 0; i < 8; i++)
				grayPattern[i] = (short)(0x5555 << (i & 1));

			var hBitmap = SafeNativeMethods.CreateBitmap(8, 8, 1, 1, grayPattern);

			try
			{
				return
					SafeNativeMethods.CreateBrushIndirect(
						new NativeMethods.LOGBRUSH
						{
							lbColor = ColorTranslator.ToWin32(Color.Black),
							lbStyle = NativeMethods.BS_PATTERN,
							lbHatch = hBitmap
						});
			}
			finally
			{
				SafeNativeMethods.DeleteObject(new HandleRef(null, hBitmap));
			}
		}
	}
	// ReSharper restore InconsistentNaming
}
