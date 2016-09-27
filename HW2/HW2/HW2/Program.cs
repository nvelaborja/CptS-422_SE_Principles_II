using System;
using CS422;
using System.Threading;

namespace HW2
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			byte[] test1 = new byte[10]{9, 3, 1, 24, 13, 23, 6, 8, 10, 5};
			byte[] test2 = new byte[10]{4, 19, 3, 12, 2, 8, 11, 6, 10, 7};

			ThreadPoolSleepSorter sleepSorter = new ThreadPoolSleepSorter (Console.Out, 10);

			Console.WriteLine ("Test 1");
			sleepSorter.Sort(test1);
		}
	}
}
