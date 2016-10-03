using System;
using System.IO;
using CS422;

namespace VelaBorja_HW5
{
	class MainClass
	{
		static string upperString = "ALL CHARS IN THIS STRING ARE UPPER CASE";
		static string lowerString = "all chars in this string are lower case";


		public static void Main (string[] args)
		{
			byte[] bytes = System.Text.Encoding.Unicode.GetBytes (upperString);
			byte[] bytes2 = System.Text.Encoding.Unicode.GetBytes (lowerString);
			byte[] results = new byte[1024];

			int read = 0;

			NoSeekMemoryStream stream = new NoSeekMemoryStream (bytes, 2, 1024);
			PrintStreamInfo ("stream", stream);

			read = stream.Read (results, 0, 1024);
			Console.WriteLine ("Read: " + read.ToString ());
			PrintBytes ("results", results);
			PrintStreamInfo ("stream", stream);

			results = new byte[1024];

			read = stream.Read (results, 0, 1024);
			Console.WriteLine ("Read: " + read.ToString ());
			PrintBytes ("results", results);
			PrintStreamInfo ("stream", stream);
		}

		private static void PrintBytes(string name, byte[] bytes)
		{
			Console.WriteLine (string.Format ("********* {0} **********", name));

			for (int i = 0; i < bytes.Length; i++) {
				if (bytes [i] == 0)
					Console.Write ("\0");
				else
					Console.Write ((char)bytes [i]);
			}
			Console.WriteLine ();

			Console.WriteLine ("***************************");
		}

		private static void PrintStreamInfo(string name, NoSeekMemoryStream stream)
		{
			Console.WriteLine(string.Format("********** {0} **********", name));
			Console.WriteLine (string.Format ("Length: {0}", stream.Length));
			Console.WriteLine (string.Format ("Position: {0}", stream.Position));
			Console.WriteLine ("***************************");
		}
	}
}
