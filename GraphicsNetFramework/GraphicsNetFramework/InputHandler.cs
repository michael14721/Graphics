using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Input;

namespace Graphics
{
	internal class InputHandler
	{
		private readonly IDictionary<Key, Action> _callback;

		public InputHandler()
		{
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

		[STAThread]
		public bool HandleInput()
		{
			bool handled = false;
			foreach (var handler in _callback)
			{
				if (Keyboard.IsKeyDown(handler.Key))
				{
					handler.Value.Invoke();
					handled = true;
				}
			}

			return handled;
		}
	}
}