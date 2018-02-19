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
		private readonly Stopwatch _stopwatchGravity;
		private readonly TimeSpan _gravityTimeSpan = TimeSpan.FromMilliseconds(200);
		private readonly TimeSpan _movementSpan = TimeSpan.FromMilliseconds(80);
		private readonly double _gravity = 10;
		private double _acceleration = 0;

		public Player(InputHandler inputHandler, CollisionMap collisionMap, Cave cave, int width)
		{
			_inputHandler = inputHandler;
			_collisionMap = collisionMap;
			_cave = cave;
			_width = width;

			_stopwatchGravity = new Stopwatch();
			_stopwatchRight = new Stopwatch();
			_stopwatchLeft = new Stopwatch();
			_stopwatchUp = new Stopwatch();
			_stopwatchDown = new Stopwatch();

			_stopwatchGravity.Start();
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

		public override void Step()
		{
			base.Step();
			if (_stopwatchGravity.Elapsed < _gravityTimeSpan)
			{
				return;
			}
			else
			{
				_acceleration += _gravity;
				switch (Math.Sign(Math.Floor(_acceleration)))
				{
					case -1:
						if (!_collisionMap.IsFree(X, Y - 1))
						{
							_acceleration = 0;
						}
						else
						{
							Y -= 1;
						}
						break;
					case 1:
						if (!_collisionMap.IsFree(X, Y + 1))
						{
							_acceleration = 0;
						}
						else
						{
							Y += 1;
						}
						break;
					default:
						break;
				}
			}

			_stopwatchGravity.Restart();
		}

		private void RegisterInputHandlers()
		{
			_inputHandler.AddHandler(Key.W, () =>
			{
				if (!_collisionMap.IsFree(X, Y + 1))
				{
					if (_stopwatchUp.Elapsed < _movementSpan)
					{
						return;
					}
					else
					{
						_acceleration = -50;
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