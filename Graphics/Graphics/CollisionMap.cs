using System.Collections.Generic;

namespace Graphics
{
	internal class CollisionMap
	{
		private readonly int _height;
		private readonly int _width;

		public CollisionMap(int width, int height)
		{
			_width = width;
			_height = height;

			Map = new List<List<bool>>(_width);

			for (var i = 0; i < _width; i++)
				Map.Add(new List<bool>(new bool[_height]));
		}

		public List<List<bool>> Map { get; }

		public void SetFree(int x, int y)
		{
			if (InsideBoundaries(x, y))
				Map[x][y] = true;
		}

		public void SetSolid(int x, int y)
		{
			if (InsideBoundaries(x, y))
				Map[x][y] = false;
		}

		public bool IsFree(int x, int y)
		{
			if (InsideBoundaries(x, y))
				return Map[x][y];
			return false;
		}

		public void ShiftLeft()
		{
			Map.RemoveAt(0);
			Map.Add(new List<bool>(new bool[_height + 1]));
		}

		private bool InsideBoundaries(int x, int y)
		{
			return (x < 0 || y < 0 || x > _width - 1 || y > _height - 1) == false;
		}
	}
}