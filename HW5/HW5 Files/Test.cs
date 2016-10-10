using NUnit.Framework;
using System;
using System.IO;
using CS422;

namespace HW5_UnitTests
{
	[TestFixture ()]
	public class Test
	{
		/// <summary>
		/// This function tests the NoSeekMemoryStream constructors, making sure they handle parameters appropriately
		/// </summary>
		[Test ()] 
		public void NoSeekMemoryStreamConstructorTest ()
		{
			string upperString = "ALL CHARS IN THIS STRING ARE UPPER CASE";
			string lowerString = "all chars in this string are lower case";

			byte[] upperBytes = System.Text.Encoding.Unicode.GetBytes (upperString);
			byte[] lowerBytes = System.Text.Encoding.Unicode.GetBytes (lowerString);

			NoSeekMemoryStream upper = new NoSeekMemoryStream (upperBytes);
			NoSeekMemoryStream lower = new NoSeekMemoryStream (lowerBytes, 4, 10);

			try{
				long length = upper.Length;
			} catch (Exception ex) {
				Assert.That (ex.GetType, Is.EqualTo (typeof(NotSupportedException)));
			}

			try{
				long position = lower.Position;
			} catch (Exception ex) {
				Assert.That (ex.GetType, Is.EqualTo (typeof(NotSupportedException)));
			}
		}

		/// <summary>
		/// This funciton tests the ConcatStream when given two MemoryStream streams 
		/// </summary>
		[Test ()]
		public void ConcatStreamTest1 ()
		{
			// Try testing out constructor stuff
			byte[] bytes1 = System.Text.Encoding.Unicode.GetBytes ("abcdefghijklmnopqrstuvwxyz");
			byte[] bytes2 = System.Text.Encoding.Unicode.GetBytes ("abcdefghijklmnopqrstuvwxyz".ToUpper());

			MemoryStream stream1 = new MemoryStream (bytes1);
			MemoryStream stream2 = new MemoryStream (bytes2);
			MemoryStream stream3 = new MemoryStream (bytes1);
			MemoryStream stream4 = new MemoryStream (bytes2);
			MemoryStream stream5 = new MemoryStream (bytes1);
			MemoryStream stream6 = new MemoryStream (bytes2);

			ConcatStream cstream1 = new ConcatStream (stream1, stream2);
			ConcatStream cstream2 = new ConcatStream (stream3, stream4, 20);
			ConcatStream cstream3 = new ConcatStream (stream5, stream6);

			byte[] results1 = new byte[1024];
			byte[] results2 = new byte[1024];

			cstream1.Seek (5, SeekOrigin.Begin);
			cstream2.Seek (-12, SeekOrigin.End);

			int br1 = cstream1.Read (results1, 0, 1024);
			int br2 = cstream2.Read (results2, 0, 1024);

			Assert.That (br1, Is.EqualTo (99));
			Assert.That (br2, Is.EqualTo (12));

			// Try reading in random chunks and make sure its all good
			Random rng = new Random();

			byte[] results3 = new byte[bytes1.Length + bytes2.Length];
			int bytesRead = 0;

			while (bytesRead < bytes1.Length + bytes2.Length) {
				int read = rng.Next (1, 10);
				//int read = 5;

				if (read > (bytes1.Length + bytes2.Length - bytesRead))
					read = bytes1.Length + bytes2.Length - bytesRead;

				bytesRead += cstream3.Read (results3, bytesRead, read);
			}

			Assert.That (System.Text.Encoding.Unicode.GetString (results3), Is.EqualTo ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"));
			//Assert.That (results3, Is.EqualTo (System.Text.Encoding.Unicode.GetBytes("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ")));
		}

		/// <summary>
		/// This function tests the ConcatStream when given a MemoryStream and NoSeekMemoryStream, respectively
		/// </summary>
		[Test ()]
		public void ConcatStreamTest2 ()
		{
			byte[] bytes1 = System.Text.Encoding.Unicode.GetBytes ("abcdefghijklmnopqrstuvwxyz");
			byte[] bytes2 = System.Text.Encoding.Unicode.GetBytes ("abcdefghijklmnopqrstuvwxyz".ToUpper());

			MemoryStream stream1 = new MemoryStream (bytes1, 10, 10);
			NoSeekMemoryStream stream2 = new NoSeekMemoryStream (bytes2);

			ConcatStream cstream = new ConcatStream (stream1, stream2);

			byte[] results = new byte[1024];

			int br1 = cstream.Read (results, 0, 1024);

			Assert.That (br1, Is.EqualTo (62));
		}

		/// <summary>
		/// This function tests the Length property functionality within a ConcatStream under the circumstances provided in ConcatStreamTest1 and ConcastStreamTest2
		/// </summary>
		[Test ()]
		public void ConcatStreamTest3 ()
		{
			byte[] bytes1 = System.Text.Encoding.Unicode.GetBytes ("abcdefghijklmnopqrstuvwxyz");
			byte[] bytes2 = System.Text.Encoding.Unicode.GetBytes ("abcdefghijklmnopqrstuvwxyz".ToUpper());

			MemoryStream stream1 = new MemoryStream (bytes1, 10, 10);
			NoSeekMemoryStream stream2 = new NoSeekMemoryStream (bytes2);
			ConcatStream cstream = new ConcatStream (stream1, stream2, 15);

			try {
				
			} catch (Exception ex) {
				Assert.That (ex.GetType (), Is.EqualTo (typeof(NotSupportedException)));
			}



			byte[] results = new byte[1024];

			int br1 = cstream.Read (results, 0, 1024);

			Assert.That (br1, Is.EqualTo (62));
		}
	}


}

