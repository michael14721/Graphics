using System;
using System.Diagnostics;

namespace Graphics.Objects
{
    internal class MessageBox : GameObject
    {
	    private int _pos;
	    private int _word;
	    private int _renderPos;
	    private int _nextArrowState;
		private string[] _message;

	    private readonly int _offset;
	    private readonly short _color;
	    private readonly TimeSpan _writeDelay;
	    private readonly TimeSpan _nextArrowDelay;
	    private readonly Stopwatch _writeTimer;
	    private readonly Stopwatch _nextArrowTimer;

	    public MessageBox()
	    {
		    _color = 5;
		    _pos = 0;
		    _word = 0;
		    _renderPos = 0;
		    _nextArrowState = 0;
		    _offset = 2;

		    _nextArrowDelay = TimeSpan.FromMilliseconds(300);
			_nextArrowTimer = new Stopwatch();

			_writeDelay = TimeSpan.FromMilliseconds(5);
			_writeTimer = Stopwatch.StartNew();

			_message = new string[0];
		}

	    public void Resize()
	    {
			Graphic = new Graphic(Width, Height);
		    Restart();
		}

	    public override void Step()
	    {
			if (_writeTimer.Elapsed >= _writeDelay &&_pos < _message[_word].Length)
			{
				Graphic.At(_renderPos + _offset, _offset).Attributes = 15;
				Graphic.At(_renderPos + _offset, _offset).Char.UnicodeChar = _message[_word][_pos];

				++_pos;
				++_renderPos;

				// End of word
				if (_pos >= _message[_word].Length)
				{
					++_word;

					// End of text
					if (_word >= _message.Length)
					{
						_writeTimer.Reset();
					}
					else
					{
						// Word wrap
						if (_renderPos % Width + _message[_word].Length > Width - 5)
						{
							_renderPos = (_renderPos / Width + 1) * Width;
						}
						else
						{
							++_renderPos;
						}

						// Limit lines
						if (_renderPos / Width > 2)
						{
							_writeTimer.Reset();
							_nextArrowTimer.Start();
						}
						else
						{
							_pos = 0;
							_writeTimer.Restart();
						}
					}
				}
			}

		    if (_nextArrowTimer.Elapsed >= _nextArrowDelay)
		    {
			    _nextArrowState = _nextArrowState == 1 ? 0 : 1;
			    
				Graphic.At(Width - _offset + _nextArrowState - 2, Height - _offset).Attributes = 15;
			    Graphic.At(Width - _offset + _nextArrowState - 2, Height - _offset).Char.AsciiChar = 175;
			    Graphic.At(Width - _offset + (1 - _nextArrowState) - 2, Height - _offset).Attributes = 0;

				_nextArrowTimer.Restart();
			}
	    }

	    public void Go()
	    {
		    if (_writeTimer.IsRunning)
			    return;

		    if (_word >= _message.Length)
		    {
			    Visible = false;
			    return;
		    }

		    _pos = 0;
		    _renderPos = 0;

		    Clean();

			_writeTimer.Start();
			_nextArrowTimer.Reset();
	    }

	    public void SetText(string text)
	    {
		    _message = text.Split(' ');
	    }

	    public void Restart()
	    {
			_pos = 0;
		    _word = 0;
		    _renderPos = 0;
			
		    Clean();

		    _writeTimer.Restart();
		    _nextArrowTimer.Reset();
		}

	    private void Clean()
	    {
			for(var x = 0; x < Graphic.Width; ++x)
		    {
				for (var y = 0; y < Graphic.Height; ++y)
				{
					Graphic.At(x, y).Attributes = _color;

					if (x == 0 || x == Width - 1)
					{
						Graphic.At(x, y).Char.UnicodeChar = '|';
					}
					else if (y == 0 || y == Height - 1)
					{
						Graphic.At(x, y).Char.UnicodeChar = '-';
					}
					else
					{
						Graphic.At(x, y).Attributes = 0;
					}
				}
			}

		    Graphic.At(0, 0).Char.UnicodeChar = '+';
		    Graphic.At(0, Height - 1).Char.UnicodeChar = '+';
		    Graphic.At(Width - 1, 0).Char.UnicodeChar = '+';
		    Graphic.At(Width - 1, Height - 1).Char.UnicodeChar = '+';
		}
    }
}