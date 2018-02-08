namespace Graphics.Objects
{
	internal class Cave : GameObject
	{
		private readonly CollisionMap _cmap;
		private readonly CaveGenerator _generator;
		private readonly ConsoleApi.CharInfo[] _graphic;

		public Cave(short width, short height, CollisionMap cmap)
		{
			_cmap = cmap;
			Width = width;
			Height = height;
			Depth = 0;
			X = 0;
			Y = 0;

			_generator = new CaveGenerator(
				caveStart: Height / 4,
				caveMinWidth: 3,
				caveMaxWidth: Height,
				caveRoughness: 50,
				caveWindyness: 50
			);

			_graphic = new ConsoleApi.CharInfo[Width * Height];

			//var caveY = height / 2;
			var caveX = 3;

			for (var i = 0; i < _graphic.Length; ++i)
			{
				_graphic[i].Attributes = 3;
				_graphic[i].Char.AsciiChar = (byte)'#';
			}

			for (var i = caveX; i < Width - 1; ++i)
			{
				var line = _generator.GenerateLine();

				for (var j = 0; j < Height; ++j)
				{
					if (j >= line.StartPosition + Height / 4 && j <= line.EndPosition + Height / 4)
					{
						_graphic[i + (Width - 1) * j].Attributes = 1;
						_graphic[i + (Width - 1) * j].Char.AsciiChar = (byte)'.';
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
				var pos = RenderGraphicPosition + (Width - 1) * j;

				if (j >= line.StartPosition + Height / 4 && j <= line.EndPosition + Height / 4)
				{
					_graphic[pos].Attributes = 2;
					_graphic[pos].Char.AsciiChar = (byte)'.';
					_cmap.SetFree(Width, j);
				}
				else
				{
					_graphic[pos].Attributes = 16;
					_graphic[pos].Char.AsciiChar = (byte)'#';
					_cmap.SetSolid(Width, j);
				}
			}

			RenderGraphicPosition = (RenderGraphicPosition + 1) % Width;
		}
	}
}