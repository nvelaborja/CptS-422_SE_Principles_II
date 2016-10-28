using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using CS422;

namespace HW8
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			MemoryFileSystem fs = new MemoryFileSystem ();
			Dir422 root = fs.GetRoot ();
			root.CreateDir ("Depth1Dir");
			root.CreateFile ("Depth1File");

			Dir422 c = root.GetDir ("Depth1Dir");
			File422 d = root.GetFile ("Depth1File");

			Stream rwStream = d.OpenReadWrite ();
			rwStream.Write (System.Text.Encoding.ASCII.GetBytes ("This is a test"), 0, 14);
			rwStream.Dispose ();

			Stream rStream = d.OpenReadOnly ();
			byte[] buffer = new byte[14];
			rStream.Read (buffer, 0, 14);

			Console.WriteLine (System.Text.Encoding.ASCII.GetString (buffer));
		}
	}
}
