using System;
using System.IO;
using CS422;

namespace VelaBorja_HW12
{
	class MainClass
	{
		static string upperString = "ALL CHARS IN THIS STRING ARE UPPER CASE";
		static string lowerString = "all chars in this string are lower case";


		public static void Main (string[] args)
		{
			//TestNoSeekMemoryStream();
			//TestTwoMemoryStreams();
			//SeekTest();
			//RNGTest();
		}

		private static void RNGTest()
		{
			byte[] bytes1 = System.Text.Encoding.Unicode.GetBytes ("abcdefghijklmnopqrstuvwxyz");
			byte[] bytes2 = System.Text.Encoding.Unicode.GetBytes ("abcdefghijklmnopqrstuvwxyz".ToUpper());

			MemoryStream stream1 = new MemoryStream (bytes1);
			NoSeekMemoryStream stream2 = new NoSeekMemoryStream (bytes2);

			ConcatStream cstream = new ConcatStream (stream1, stream2);

			Random rng = new Random();

			byte[] results = new byte[bytes1.Length + bytes2.Length];
			int bytesRead = 0;

			while (bytesRead < bytes1.Length + bytes2.Length) {
				int read = rng.Next (1, 10);
				//int read = 5;

				if (read > (bytes1.Length + bytes2.Length - bytesRead))
					read = bytes1.Length + bytes2.Length - bytesRead;

				bytesRead += cstream.Read (results, bytesRead, read);

				PrintBytes ("results", results);
			}

			int br1 = cstream.Read (results, 0, bytes1.Length + bytes2.Length);

			PrintBytes ("results", results);
			Console.WriteLine (string.Format ("Bytes read: {0}", br1));
		}

		private static void SeekTest()
		{
			byte[] bytes1 = System.Text.Encoding.Unicode.GetBytes ("abcdefghijklmnopqrstuvwxyz");
			byte[] bytes2 = System.Text.Encoding.Unicode.GetBytes ("abcdefghijklmnopqrstuvwxyz".ToUpper());

			MemoryStream stream1 = new MemoryStream (bytes1, 10, 10);
			NoSeekMemoryStream stream2 = new NoSeekMemoryStream (bytes2);

			ConcatStream cstream = new ConcatStream (stream1, stream2);

			byte[] results = new byte[1024];

			cstream.Seek (5, SeekOrigin.Begin);

			int br1 = cstream.Read (results, 0, 1024);

			PrintBytes ("results", results);
			Console.WriteLine (string.Format ("Bytes read: {0}", br1));
		}

		private static void TestTwoMemoryStreams()
		{
			byte[] bytes1 = System.Text.Encoding.Unicode.GetBytes ("abcdefghijklmnopqrstuvwxyz");
			byte[] bytes2 = System.Text.Encoding.Unicode.GetBytes ("abcdefghijklmnopqrstuvwxyz".ToUpper());

			MemoryStream stream1 = new MemoryStream (bytes1);
			MemoryStream stream2 = new MemoryStream (bytes2);
			MemoryStream stream3 = new MemoryStream (bytes1);
			MemoryStream stream4 = new MemoryStream (bytes2);

			ConcatStream cstream1 = new ConcatStream (stream1, stream2);
			ConcatStream cstream2 = new ConcatStream (stream3, stream4, 20);

			byte[] results1 = new byte[1024];
			byte[] results2 = new byte[1024];

			cstream1.Seek (5, SeekOrigin.Begin);
			cstream2.Seek (-12, SeekOrigin.End);

			int br1 = cstream1.Read (results1, 0, 1024);
			int br2 = cstream2.Read (results2, 0, 1024);

			PrintBytes ("results1", results1);
			Console.WriteLine (string.Format ("Bytes read: {0}", br1));

			PrintBytes ("results2", results2);
			Console.WriteLine (string.Format ("Bytes read: {0}", br2));
		}

		private static void TestNoSeekMemoryStream()
		{
			byte[] bytes = System.Text.Encoding.Unicode.GetBytes (upperString);
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
