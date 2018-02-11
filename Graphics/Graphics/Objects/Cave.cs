namespace Graphics.Objects
{
	internal class Cave : GameObject
	{
		private readonly CollisionMap _cmap;
		private readonly CaveGenerator _generator;
		private readonly Graphic _graphic;

		public Cave(int width, int height, CollisionMap cmap)
		{
			_cmap = cmap;
			Width = width;
			Height = height;

			_generator = new CaveGenerator(
				Height / 2,
				3,
				Height,
				50,
				50
			);

			_graphic = new Graphic(Width, Height);

			for (var i = 0; i < _graphic.Surface.Length; ++i)
			{
				_graphic.Surface[i].Attributes = 3;
				_graphic.Surface[i].Char.UnicodeChar = '#';
			}

			const int startX = 3;

			for (var i = 0; i < Width; ++i)
				if (i >= startX)
				{
					var line = _generator.GenerateLine();

					for (var j = 0; j < Height; ++j)
						if (j >= line.StartPosition
						    && j <= line.EndPosition)
						{
							_graphic.At(i, j).Attributes = 1;
							_graphic.At(i, j).Char.UnicodeChar = '.';
							cmap.SetFree(i, j);
						}
				}

			Graphic = _graphic;
		}

		public void ExpandOne()
		{
			var line = _generator.GenerateLine();

			for (var j = 0; j < Height; ++j)
				if (j >= line.StartPosition && j <= line.EndPosition)
				{
					_graphic.At(RenderGraphicPosition, j).Attributes = 1;
					_graphic.At(RenderGraphicPosition, j).Char.UnicodeChar = '.';
					_cmap.SetFree(Width - 1, j);
				}
				else
				{
					_graphic.At(RenderGraphicPosition, j).Attributes = 3;
					_graphic.At(RenderGraphicPosition, j).Char.UnicodeChar = '#';
					_cmap.SetSolid(Width - 1, j);
				}

			RenderGraphicPosition = (RenderGraphicPosition + 1) % Width;
		}
	}
}