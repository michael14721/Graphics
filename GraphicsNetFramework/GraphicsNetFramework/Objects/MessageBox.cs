using System;
using System.Diagnostics;

namespace Graphics.Objects
{
	internal class MessageBox : GameObject
	{
		private const short TextColor = 15;				// Color of the text
		private const short MsgBoxColor = 5;			// Color of the messagebox

		private const char WaitForInputChar = '\n';		// A character in the message which tells the message box to put no more text in the current screen. Has to be surrounded by spaces.

		private readonly TimeSpan _nextArrowDelay;
		private readonly Stopwatch _nextArrowTimer;
		private readonly int _offset;					// Offset from borders of graphic

		private readonly TimeSpan _writeDelay;
		private readonly Stopwatch _writeTimer;
		private string[] _message;						// The message to show
		private int _nextArrowState;					// 1 or 0 depending on which state the 'next-arrow' is in (right or left)
		private int _pos;								// Position inside current word
		private int _renderPos;							// Position inside Graphic to render the next letter
		private int _word;                              // Current word to be read
		private bool _skip;

		public MessageBox()
		{
			_pos = 0;
			_word = 0;
			_renderPos = 0;
			_nextArrowState = 0;
			_offset = 2;
			_skip = false;

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
			if (!_skip)
			{
				if (_writeTimer.Elapsed >= _writeDelay && _word < _message.Length && _pos < _message[_word].Length)
				{
					if (_message[_word][0] == WaitForInputChar)
					{
						WaitForInput();
						return;
					}

					Graphic.Set(_renderPos + _offset, _offset, TextColor, _message[_word][_pos]);

					++_pos;
					++_renderPos;

					// End of word
					if (_pos >= _message[_word].Length)
					{
						++_word;
						_pos = 0;

						// End of text
						if (_word >= _message.Length)
						{
							_writeTimer.Reset();
						}
						else
						{
							// Word wrap
							if (_renderPos % Width + _message[_word].Length > Width - 5)
								_renderPos = (_renderPos / Width + 1) * Width;
							else
								++_renderPos;

							// Check if line limit has been reached before rendering next letter
							if (_renderPos / Width > 2)
								WaitForInput();
							else
								_writeTimer.Restart();
						}
					}
				}

				// Animate the 'next-arrow'
				if (_nextArrowTimer.Elapsed >= _nextArrowDelay)
				{
					_nextArrowState = _nextArrowState == 1 ? 0 : 1;

					Graphic.Set(Width - _offset + _nextArrowState - 2, Height - _offset, TextColor, 175);
					Graphic.At(Width - _offset + (1 - _nextArrowState) - 2, Height - _offset).Attributes = 0;

					_nextArrowTimer.Restart();
				}
			}
		}

		public void Go()
		{
			// Already writing to screen
			if (_writeTimer.IsRunning && _skip == false)
			{
				_skip = true;
				while (_message[_word][0] != WaitForInputChar && _renderPos / Width <= 2 && _word < _message.Length && _pos < _message[_word].Length)
				{		
					Graphic.Set(_renderPos + _offset, _offset, TextColor, _message[_word][_pos]);

					++_pos;
					++_renderPos;

					// End of word
					if (_pos >= _message[_word].Length)
					{
						++_word;
						_pos = 0;

						// End of message
						if (_word >= _message.Length)
						{
							break;
						}

						// Word wrap
						if (_renderPos % Width + _message[_word].Length > Width - 5)
							_renderPos = (_renderPos / Width + 1) * Width;
						else
							++_renderPos;
					}
				}

				WaitForInput();
				_skip = false;
				return;
			}

			// Disappear when end of text is reached
			if (_word >= _message.Length)
			{
				Visible = false;
				Stop();
				return;
			}

			// Skip the current escape character because it already has proc'd
			if (_message[_word][0] == WaitForInputChar)
			{
				++_word;
				_pos = 0;
			}

			_renderPos = 0;

			Clean();

			_writeTimer.Restart();
			_nextArrowTimer.Reset();
		}

		private void WaitForInput()
		{
			_writeTimer.Reset();
			_nextArrowTimer.Start();
		}

		public void SetText(string text)
		{
			_message = text.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);
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
			for (var x = 0; x < Graphic.Width; ++x)
			for (var y = 0; y < Graphic.Height; ++y)
			{
				Graphic.At(x, y).Attributes = 0;

				if (x == 0 || x == Width - 1)
				{
					Graphic.Set(x, y, MsgBoxColor, '|');
				}
				else if (y == 0 || y == Height - 1)
				{
					Graphic.Set(x, y, MsgBoxColor, '-');
				}
			}

			Graphic.At(0, 0).Char.UnicodeChar = '+';
			Graphic.At(0, Height - 1).Char.UnicodeChar = '+';
			Graphic.At(Width - 1, 0).Char.UnicodeChar = '+';
			Graphic.At(Width - 1, Height - 1).Char.UnicodeChar = '+';
		}

		private void Stop()
		{
			_writeTimer.Reset();
			_nextArrowTimer.Reset();
		}
	}
}