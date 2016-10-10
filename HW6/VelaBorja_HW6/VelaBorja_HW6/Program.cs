using System;
using CS422;

namespace VelaBorja_HW6
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Starting Web Server..");

			WebServer.Start (4000, 30);

			DemoService service = new DemoService ();

			WebServer.AddService (service);
		}
	}
}
