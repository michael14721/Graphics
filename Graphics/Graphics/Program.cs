using System;

namespace Graphics
{
	internal class Program
    {
	    internal static void Main(string[] args)
	    {
		    const int fps = 60;

			var width = (short) (Console.LargestWindowWidth / 3);
			var height = (short) (Console.LargestWindowHeight / 3);

		    Console.SetWindowSize(width, height);
			Console.SetBufferSize(width, height);
			Console.CursorVisible = false;

		    var r = new Renderer();

		    if (r.FileHandleIsValid())
		    {
				var game = new Game(width, height, fps, r);
				game.Run();
		    }

		    Console.ReadKey();
	    }
	}
}