using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using Graphics.Objects;
using GraphicsNetFramework;

namespace Graphics
{
	internal class Game
	{
		private readonly int _fps;
		private readonly int _height;
		private readonly Renderer _renderer;
		private readonly int _width;
		private CollisionMap _cmap;
		private GraphicManager _graphicManager;
		private InputHandler _inputHandler;
		private Stopwatch _sw;

		public Game(int width, int height, int fps, Renderer renderer)
		{
			_width = width;
			_height = height;
			_fps = fps;
			_renderer = renderer;

			Initialize();
		}

		private void Initialize()
		{
			_inputHandler = new InputHandler();
			_sw = Stopwatch.StartNew();

			_cmap = new CollisionMap(_width, _height);
			var cave = new Cave(_width, _height, _cmap);
			var msgBox = new MessageBox(_inputHandler)
			{
				Depth = -90,
				X = 2,
				Y = _height - 9,
				Width = _width - 4,
				Height = 7
			};

			msgBox.SetText(
				"Lorem ipsum dolor sit amet, \n consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. \n Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
			msgBox.Resize();

			var player = new Player(_inputHandler, _cmap, cave, _width)
			{
				X = 3,
				Y = _height / 2
			};

			_graphicManager = new GraphicManager(0, 0, _width, _height, _renderer);
			_graphicManager.AddGraphic(player);
			_graphicManager.AddGraphic(cave);
			_graphicManager.AddGraphic(msgBox);

			// Keybindings

			_inputHandler.AddHandler(Key.Escape, () => Environment.Exit(0));
			
			_graphicManager.UpdateDepth();
		}
		
		public void Run()
		{
			while (true)
				if (_sw.ElapsedMilliseconds >= 1000 / _fps)
				{
					_inputHandler.HandleInput();
					_graphicManager.Fill();

					//_graphicManager.ApplyFilter(surface =>
					//{
					//	for (var i = 0; i < _width; ++i)
					//	{
					//		for (var j = 0; j < _height; ++j)
					//		{
					//			if (_cmap.Map[i][j] == false)
					//				surface.At(i, j).Attributes = 10;
					//		}
					//	}
					//});

					_graphicManager.Render(_renderer);
					_sw.Restart();
				}
		}
	}
}