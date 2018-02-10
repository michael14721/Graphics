namespace Graphics.Objects
{
	internal class Player : GameObject
	{
		public Player()
		{
			Depth = -1;
			Width = 1;
			Height = 1;

			var sprite = new Graphic(1, 1);
			sprite.Surface[0].Attributes = 11;
			sprite.Surface[0].Char.UnicodeChar = 'A';
			Graphic = sprite;
		}
	}
}