using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GraphicsNetFramework
{
	class StaThreadHandler
	{
		private BlockingCollection<Func<bool>> _queue;
		private BlockingCollection<bool> _return;

		public StaThreadHandler()
		{
			_queue = new BlockingCollection<Func<bool>>();
			_return = new BlockingCollection<bool>();
			Thread thread = new Thread(Loop);
			thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
			thread.Start();
		}

		public bool Result()
		{
			return _return.Take();
		}

		public void Run(Func<bool> action)
		{
			_queue.Add(action);
		}

		private void Loop()
		{
			while (true)
			{
				_return.Add(_queue.Take().Invoke());
			}
		}
	}
}
