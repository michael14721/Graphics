using System;
using System.IO;
using Microsoft.Win32.SafeHandles;

namespace Graphics
{
	internal class Renderer
	{
		private readonly SafeFileHandle _h;

		public Renderer()
		{
			_h = ConsoleApi.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
		}

		public bool DrawRect(ConsoleApi.CharInfo[] content, int x, int y, int width, int height)
		{
			var xx = (short) x;
			var yy = (short) y;
			var w = (short) (width - 1);
			var h = (short) (height - 1);
				
			var rec = new ConsoleApi.SmallRect { Left = xx, Top = yy, Right = xx += w, Bottom = yy += h };
			return ConsoleApi.WriteConsoleOutput(_h, content, new ConsoleApi.Coord(xx, yy), new ConsoleApi.Coord(0, 0), ref rec);
		}
		
		public bool FileHandleIsValid()
		{
			return !_h.IsInvalid;
		}
	}
}