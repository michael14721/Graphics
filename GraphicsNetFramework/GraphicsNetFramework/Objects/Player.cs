using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Graphics.Objects
{
	internal class Player : GameObject
	{
		private readonly InputHandler _inputHandler;
		private readonly CollisionMap _collisionMap;
		private readonly Cave _cave;
		private readonly int _width;
		private readonly Stopwatch _stopwatchUp;
		private readonly Stopwatch _stopwatchDown;
		private readonly Stopwatch _stopwatchLeft;
		private readonly Stopwatch _stopwatchRight;
		private readonly TimeSpan _movementSpan = TimeSpan.FromMilliseconds(80);

		public Player(InputHandler inputHandler, CollisionMap collisionMap, Cave cave, int width)
		{
			_inputHandler = inputHandler;
			_collisionMap = collisionMap;
			_cave = cave;
			_width = width;

			_stopwatchRight = new Stopwatch();
			_stopwatchLeft = new Stopwatch();
			_stopwatchUp = new Stopwatch();
			_stopwatchDown = new Stopwatch();

			_stopwatchRight.Start();
			_stopwatchLeft.Start();
			_stopwatchUp.Start();
			_stopwatchDown.Start();

			Depth = -1;
			Width = 1;
			Height = 1;

			var sprite = new Graphic(1, 1);
			sprite.Surface[0].Attributes = 11;
			sprite.Surface[0].Char.UnicodeChar = 'A';
			Graphic = sprite;

			RegisterInputHandlers();
		}
		
		private void RegisterInputHandlers()
		{
			_inputHandler.AddHandler(Key.W, () =>
			{
				if (_collisionMap.IsFree(X, Y - 1))
				{
					if (_stopwatchUp.Elapsed < _movementSpan)
					{
						return;
					}
					else
					{
						Y -= 1;
						_stopwatchUp.Restart();
					}
				}
			});

			_inputHandler.AddHandler(Key.A, () =>
			{
				if (_collisionMap.IsFree(X - 1, Y))
				{
					if (_stopwatchLeft.Elapsed < _movementSpan)
					{
						return;
					}
					else
					{
						X -= 1;
						_stopwatchLeft.Restart();
					}
				}
			});

			_inputHandler.AddHandler(Key.S, () =>
			{
				if (_collisionMap.IsFree(X, Y + 1))
				{
					if (_stopwatchDown.Elapsed < _movementSpan)
					{
						return;
					}
					else
					{
						Y += 1;
						_stopwatchDown.Restart();
					}
				}
			});

			_inputHandler.AddHandler(Key.D, () =>
			{
				if (_collisionMap.IsFree(X + 1, Y))
				{
					if (_stopwatchRight.Elapsed < _movementSpan)
					{
						return;
					}
					else
					{
						if (X >= _width / 2)
						{
							_collisionMap.ShiftLeft();
							_cave.ExpandOne();
						}
						else
						{
							X += 1;
						}
						_stopwatchRight.Restart();
					}
				}
			});
		}
	}
}