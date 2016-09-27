using System;
using System.IO;
using CS422;

namespace HW1ConsoleTests
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			long streamLength = 512;
			byte[] buffer = new byte[1024];
			int bytesRead = 0;
			long seekTest = -1;

			NumberedTextWriter numberedTextWriter = new NumberedTextWriter(Console.Out, 0);
			IndexedNumsStream indexedNumsStream = new IndexedNumsStream (streamLength);

			seekTest = indexedNumsStream.Seek (5000, SeekOrigin.Begin);

			bytesRead = indexedNumsStream.Read (buffer, 0, 4000);

			int i = 0;

			while (i <  buffer.Length) 
			{
				numberedTextWriter.WriteLine(buffer[i].ToString());
				i++;
			}

			numberedTextWriter.WriteLine ("Bytes Read: " + bytesRead.ToString ());
			numberedTextWriter.WriteLine ("Seek Test: " + seekTest.ToString ());
			numberedTextWriter.WriteLine ("Position: " + indexedNumsStream.Position.ToString ());
		}
	}
}
