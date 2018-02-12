namespace Graphics.Objects
{
	internal class Cave : GameObject
	{
		private readonly CollisionMap _cmap;
		private readonly CaveGenerator _generator;

		public Cave(int width, int height, CollisionMap cmap)
		{
			_cmap = cmap;
			Width = width;
			Height = height;

			Graphic = new Graphic(Width, Height);

			_generator = new CaveGenerator(Height / 2, 3, Height, 50, 50);

			for (var i = 0; i < Width; ++i)
				SetColumn(i, i);
		}

		public void ExpandOne()
		{
			SetColumn(RenderGraphicPosition++, Width - 1);
		}

		private void SetColumn(int x, int cmapX)
		{
			var line = _generator.GenerateLine();

			for (var j = 0; j < Height; ++j)
				if (j >= line.StartPosition && j <= line.EndPosition)
				{
					Graphic.Set(x, j, 1, '.');
					_cmap.SetFree(cmapX, j);
				}
				else
				{
					Graphic.Set(x, j, 3, '#');
					_cmap.SetSolid(cmapX, j);
				}
		}
	}
}