﻿using System;

namespace Graphics
{
	internal abstract class GameObject
	{
		private int _renderGraphicPosition;

		public Guid Id { get; } = Guid.NewGuid();

		public int Depth { get; set; } = 0;

		public int X { get; set; } = 0;

		public int Y { get; set; } = 0;

		public int Width { get; set; } = 0;

		public int Height { get; set; } = 0;

		public bool Visible { get; set; } = true;

		public Graphic Graphic { get; set; }

		public int RenderGraphicPosition
		{
			get => _renderGraphicPosition;
			set => _renderGraphicPosition = value % Width;
		}

		public virtual void Step()
		{
		}
	}
}