namespace Graphics.Objects
{
	internal class Player : GameObject
	{
		public Player()
		{
			Depth = -1;
			Width = 1;
			Height = 1;

			var sprite = new ConsoleApi.CharInfo[1];
			sprite[0].Attributes = 11;
			sprite[0].Char.AsciiChar = (byte)'A';
			Graphic = sprite;
		}
	}
}