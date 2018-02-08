using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Graphics
{
    internal class InputHandler
    {
	    private ConsoleKeyInfo _key;
	    private readonly IDictionary<ConsoleKey, Action> _callback;

		public InputHandler()
		{
			_callback = new ConcurrentDictionary<ConsoleKey, Action>();
		}

	    public void AddHandler(ConsoleKey key, Action action)
	    {
		    _callback.Add(key, action);
	    }

	    public void RemoveHandlers(ConsoleKey key)
	    {
		    _callback.Remove(key);
	    }

	    public bool HandleInput()
	    {
		    if (Console.KeyAvailable)
			    _key = Console.ReadKey(true);
		    else
			    return false;

		    var r = _callback.TryGetValue(_key.Key, out var action);

			if (r)
				action.Invoke();

		    return r;
		}
    }
}
