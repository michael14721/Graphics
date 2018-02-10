using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphics
{
	internal class GraphicManager
	{
		private readonly int _height;
		private readonly Renderer _renderer;
		private readonly Graphic _surface;
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
			_surface = new Graphic(_width, _height);
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

		public void ApplyFilter(Action<Graphic> filter)
		{
			filter.Invoke(_surface);
		}

		public void Fill()
		{
			foreach (var o in _objects)
			{
				if (o.Visible == false)
					continue;

				o.Step();
				
				for (var x = 0; x < o.Graphic.Width; ++x)
				{
					for (var y = 0; y < o.Graphic.Height; ++y)
					{
						var tx = o.X + x;
						var ty = o.Y + y;

						if (tx < 0 || tx > _width - 1 || ty < 0 || ty > _height - 1)
							continue;

						_surface.At(tx, ty) = o.Graphic.At((x + o.RenderGraphicPosition) % o.Width, y);
					}
				}
			}
		}

		public void Render(Renderer r)
		{
			_renderer.DrawRect(_surface.Surface, _x, _y, _width, _height);
		}
	}
}