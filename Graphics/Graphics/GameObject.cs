using System;

namespace Graphics
{
	internal abstract class GameObject
	{
		public Guid Id { get; } = Guid.NewGuid();

		public int Depth { get; set; } = 0;

		public int X { get; set; } = 0;

		public int Y { get; set; } = 0;

		public int Width { get; set; } = 0;

		public int Height { get; set; } = 0;

		public ConsoleApi.CharInfo[] Graphic { get; set; }

		public int RenderGraphicPosition { get; set; } = 0;
	}
}