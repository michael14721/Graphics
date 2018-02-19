using System;

namespace Graphics
{
	internal class Program
	{
		internal static void Main(string[] args)
		{
			const int fps = 60;
			
			var width = (short) Math.Max(Console.LargestWindowWidth / 3, 82);
			var height = (short) Math.Max(Console.LargestWindowHeight / 3, 48);

			Console.SetWindowSize(width, height);
			Console.SetBufferSize(width, height);
			Console.CursorVisible = false;

			var r = new Renderer();

			if (r.FileHandleIsValid())
				new Game(width, height, fps, r).Run();

			Console.ReadKey();
		}
	}
}