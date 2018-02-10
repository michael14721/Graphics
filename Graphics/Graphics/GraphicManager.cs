using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphics
{
	internal class GraphicManager
	{
		private readonly int _height;
		private readonly Renderer _renderer;
		private readonly ConsoleApi.CharInfo[] _surface;
		private readonly int _width;
		private readonly int _x;
		private readonly int _y;
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

		public void ApplyFilter(Action<ConsoleApi.CharInfo[]> filter)
		{
			filter.Invoke(_surface);
		}

		public void Fill()
		{
			foreach (var t in _objects)
			{
				var c = t.X + _width * t.Y;

				for (var i = 0; i < t.Graphic.Length; ++i)
				{
					var fromx = (i + t.RenderGraphicPosition) % t.Width;
					var destx = i % t.Width;
					var desty = _width * (i / t.Width);

					if (t.X + destx > _width - 1 || t.Y + (i / t.Width) > _height - 1)
						continue;

					_surface[c + destx + desty] = t.Graphic[fromx + desty];
				}
			}
		}

		public void Render(Renderer r)
		{
			_renderer.DrawRect(_surface, _x, _y, _width, _height);
		}
	}
}