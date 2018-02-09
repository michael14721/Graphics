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
			var rec = new ConsoleApi.SmallRect
			{
				Left = (short) x,
				Top = (short) y,
				Right = (short) (x + width),
				Bottom = (short) (y + height)
			};

			return ConsoleApi.WriteConsoleOutput(_h, content, new ConsoleApi.Coord((short) width, (short) height), new ConsoleApi.Coord(0, 0), ref rec);
		}
		
		public bool FileHandleIsValid()
		{
			return !_h.IsInvalid;
		}
	}
}