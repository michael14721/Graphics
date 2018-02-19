using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using GraphicsNetFramework;

namespace Graphics
{
	internal class InputHandler
	{
		private readonly IDictionary<Key, Action> _callback;
		private readonly StaThreadHandler _handler;

		public InputHandler()
		{
			_handler = new StaThreadHandler();
			_callback = new ConcurrentDictionary<Key, Action>();
		}
		
		public void AddHandler(Key key, Action action)
		{
			_callback.Add(key, action);
		}

		public void RemoveHandlers(Key key)
		{
			_callback.Remove(key);
		}

		public bool HandleInput()
		{
			bool handled = false;

			foreach (var handler in _callback)
			{
				_handler.Run(() => Keyboard.IsKeyDown(handler.Key));
				if (_handler.Result())
				{
					handler.Value.Invoke();
					handled = true;
				}
			}
			
			return handled;
		}
	}
}