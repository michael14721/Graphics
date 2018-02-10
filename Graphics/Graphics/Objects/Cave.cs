namespace Graphics.Objects
{
	internal class Cave : GameObject
	{
		private readonly CollisionMap _cmap;
		private readonly CaveGenerator _generator;
		private readonly ConsoleApi.CharInfo[] _graphic;

		public Cave(int width, int height, CollisionMap cmap)
		{
			_cmap = cmap;
			Width = width;
			Height = height;

			_generator = new CaveGenerator(
				caveStart: Height / 4,
				caveMinWidth: 3,
				caveMaxWidth: Height,
				caveRoughness: 50,
				caveWindyness: 50
			);

			_graphic = new ConsoleApi.CharInfo[Width * Height];
			
			for (var i = 0; i < _graphic.Length; ++i)
			{
				_graphic[i].Attributes = 3;
				_graphic[i].Char.AsciiChar = (byte)'#';
			}

			var startX = 3;

			for (var i = 0; i < Width; ++i)
			{
				var line = _generator.GenerateLine();

				if (i >= startX)
					for (var j = 0; j < Height; ++j)
					{
						if (j >= line.StartPosition + Height / 4
							&& j <= line.EndPosition + Height / 4)
						{
							_graphic[i + Width * j].Attributes = 1;
							_graphic[i + Width * j].Char.AsciiChar = (byte)'.';
							cmap.SetFree(i, j);
						}
					}
			}

			Graphic = _graphic;
		}

		public void ExpandOne()
		{
			var line = _generator.GenerateLine();

			for (var j = 0; j < Height; ++j)
			{
				var pos = RenderGraphicPosition + Width * j;

				if (j >= line.StartPosition + Height / 4 && j <= line.EndPosition + Height / 4)
				{
					_graphic[pos].Attributes = 1;
					_graphic[pos].Char.AsciiChar = (byte)'.';
					_cmap.SetFree(Width - 1, j);
				}
				else
				{
					_graphic[pos].Attributes = 3;
					_graphic[pos].Char.AsciiChar = (byte)'#';
					_cmap.SetSolid(Width - 1, j);
				}
			}

			RenderGraphicPosition = (RenderGraphicPosition + 1) % Width;
		}
	}
}