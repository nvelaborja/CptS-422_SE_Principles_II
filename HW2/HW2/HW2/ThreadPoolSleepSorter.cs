using System;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;

namespace CS422
{
	public class ThreadPoolSleepSorter : IDisposable
	{
		private TextWriter output;
		private BlockingCollection<Delegate> delegateCollection;

		private delegate void SleepSortTask ();

		public ThreadPoolSleepSorter (TextWriter output, ushort threadCount)
		{
			this.output = output;
			delegateCollection = new BlockingCollection<Delegate> ();

			if (threadCount < 1)
				threadCount = 64;

			for (int i = 0; i < threadCount; i++) {			// Create threadCount threads and store them in pool
				Thread newThread = new Thread( new ThreadStart(() => { Ready(delegateCollection); }));
				newThread.Start ();
			}
		}

		private void Ready(BlockingCollection<Delegate> collection)
		{
			delegateCollection.Take ().DynamicInvoke ();
		}

		public void Sort(byte[] values)
		{
			foreach (byte value in values) {
				SleepSortTask sleepSortTask = new SleepSortTask (() => PrintAfterWait (value));
				delegateCollection.Add (sleepSortTask);
			}

		}

		/// <summary>
		/// Prints value x after waiting x seconds
		/// </summary>
		/// <returns><c>true</c>, if printed successfully, <c>false</c> otherwise.</returns>
		/// <param name="x">The value x.</param>
		private bool PrintAfterWait(int x)
		{
			try {
				Thread.Sleep (x * 1000);			// First wait x seconds
				output.WriteLine(x.ToString());		// Then write x to output

				return true;
			} catch {
				return false;
			}
		}

		public void Dispose()
		{
			delegateCollection.Dispose ();
		}
	}
}

