using System;
using System.Diagnostics;
using Graphics.Objects;

namespace Graphics
{
    internal class Game
    {
	    private readonly int _width;
	    private readonly int _height;
	    private readonly int _fps;
	    private readonly Renderer _renderer;
	    private Stopwatch _sw;
	    private InputHandler _inputHandler;
	    private GraphicManager _graphicManager;
	    private CollisionMap _cmap;

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
			_sw = Stopwatch.StartNew();

		    _cmap = new CollisionMap(_width, _height);
		    var cave = new Cave(_width, _height, _cmap);

		    var player = new Player
		    {
			    X = 3,
			    Y = _height / 2
		    };

		    _graphicManager = new GraphicManager(0, 0, _width, _height, _renderer);
			_graphicManager.AddGraphic(player);
		    _graphicManager.AddGraphic(cave);

		    // Keybindings
		    _inputHandler = new InputHandler();

		    _inputHandler.AddHandler(ConsoleKey.Escape, () => Environment.Exit(0));

		    _inputHandler.AddHandler(ConsoleKey.W, () =>
		    {
			    if (_cmap.IsFree(player.X, player.Y - 1))
				    player.Y -= 1;
		    });

		    _inputHandler.AddHandler(ConsoleKey.A, () =>
		    {
			    if (_cmap.IsFree(player.X - 1, player.Y))
				    player.X -= 1;
		    });

		    _inputHandler.AddHandler(ConsoleKey.S, () =>
		    {
			    if (_cmap.IsFree(player.X, player.Y + 1))
				    player.Y += 1;
		    });

		    _inputHandler.AddHandler(ConsoleKey.D, () =>
		    {
			    if (_cmap.IsFree(player.X + 1, player.Y))
			    {
				    if (player.X >= _width / 2)
				    {
					    _cmap.ShiftLeft();
					    cave.ExpandOne();
				    }
				    else
					    player.X += 1;
			    }
		    });

		    _inputHandler.AddHandler(ConsoleKey.NumPad1, () =>
		    {
				_cmap.ShiftLeft();
				cave.ExpandOne();
		    });

		    _inputHandler.AddHandler(ConsoleKey.NumPad2, () =>
		    {
				player.RenderGraphicPosition = (player.RenderGraphicPosition + 1) % _width;
			    cave.RenderGraphicPosition = (cave.RenderGraphicPosition + 1) % _width;
		    });

			_graphicManager.UpdateDepth();
		}

		public void Run()
	    {
			while (true)
			{
				if (_sw.ElapsedMilliseconds >= 1000 / _fps)
				{
					_inputHandler.HandleInput();
					_graphicManager.Fill();

					_graphicManager.ApplyFilter(surface =>
					{
						for (var i = 0; i < _width; ++i)
						{
							for (var j = 0; j < _height; ++j)
							{
								if (_cmap.Map[i][j] == false)
									surface[i + j * _width].Attributes += 1;
							}
						}
					});

					_graphicManager.Render(_renderer);
					_sw.Restart();
				}
			}
		}
    }
}