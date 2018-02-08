using System.Collections.Generic;
using System.Linq;

namespace Graphics
{
	internal class GraphicManager
	{
		private readonly int _x;
		private readonly int _y;
		private readonly int _width;
		private readonly int _height;
		private readonly Renderer _renderer;
		public readonly ConsoleApi.CharInfo[] _surface;
		private List<GameObject> _objects;

		public GraphicManager(int x, int y, int width, int height, Renderer renderer)
		{
			_x = x;
			_y = y;
			_width = width;
			_height = height;
			_renderer = renderer;

			_objects = new List<GameObject>();
			_surface = new ConsoleApi.CharInfo[_width * _height];
		}

		public void UpdateDepth()
		{
			_objects = _objects.OrderByDescending(g => g.Depth).ToList();
		}

		public void AddGraphic(GameObject obj)
		{
			_objects.Add(obj);
		}

		public void RemoveGraphic(GameObject obj)
		{
			_objects.Remove(obj);
		}

		public void Fill()
		{
			foreach (var t in _objects)
			{
				var c = t.X + (_width - 1) * t.Y;

				for (var i = 0; i < t.Graphic.Length; ++i)
				{
					var tposy = _width * (i / t.Width);
					var tposx = i - t.RenderGraphicPosition + _width;
					//var tposx = i + (_width - t.RenderGraphicPosition) % _width;

					if ((t.X + (i % t.Width)) > _width)
						tposy -= _width;

					tposx %= _width;
					
					_surface[c + tposx + tposy] = t.Graphic[i];
				}
			}
		}

		public void Render(Renderer r)
		{
			_renderer.DrawRect(_surface, _x, _y, _width, _height);
		}
	}
}