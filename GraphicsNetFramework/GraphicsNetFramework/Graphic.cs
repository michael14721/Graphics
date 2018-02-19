namespace Graphics
{
	internal class Graphic
	{
		public Graphic(int width, int height)
		{
			Width = width;
			Height = height;

			Surface = new ConsoleApi.CharInfo[Width * Height];
		}

		public int Width { get; }

		public int Height { get; }

		public ConsoleApi.CharInfo[] Surface { get; }

		public ref ConsoleApi.CharInfo At(int x, int y)
		{
			return ref Surface[x + y * Width];
		}
	}
}