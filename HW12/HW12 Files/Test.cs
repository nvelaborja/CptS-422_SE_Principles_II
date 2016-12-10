using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using CS422;

namespace HW12_UnitTests
{
	[TestFixture ()]
	public class Test
	{
		/// <summary>
		/// This function tests the NoSeekMemoryStream constructors, making sure they handle parameters appropriately
		/// </summary>
		[Test ()] 
		public void FirstConstructorTest ()
		{
			string line = "abcdefg";
			MemoryStream mStream1 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			MemoryStream mStream2 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			NoSeekMemoryStream nStream1 = new NoSeekMemoryStream(Encoding.ASCII.GetBytes (line));

			try {
				ConcatStream cStream1 = new ConcatStream(nStream1, mStream1);
				cStream1.Flush();
			} catch (Exception e) {
				Assert.That (e.Message == "First Stream in ConcatStream does not have specified length.", "Expecting cStream1 to raise ArgumentException with message 'First Stream in ConcatStream does not have specified length.'");
			}

			ConcatStream cStream2 = new ConcatStream (mStream1, mStream2);
			Assert.That (cStream2.CanSeek == true, "Expecting cStream2.CanSeek == true");
			Assert.That (cStream2.HasLength == true, "Expecting cStream2.HasLength == true");

			ConcatStream cStream3 = new ConcatStream (mStream1, nStream1);
			Assert.That (cStream3.CanSeek == false, "Expecting cStream3.CanSeek == false");
			Assert.That (cStream3.HasLength == false, "Expecting cStream3.HasLength == false");
		}

		[Test ()] 
		public void SecondConstructorTest ()
		{
			string line = "abcdefg";
			MemoryStream mStream1 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			MemoryStream mStream2 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			NoSeekMemoryStream nStream1 = new NoSeekMemoryStream(Encoding.ASCII.GetBytes (line));

			try {
				ConcatStream cStream1 = new ConcatStream(nStream1, mStream1, 12);
				cStream1.Flush();
			} catch (Exception e) {
				Assert.That (e.Message == "First Stream in ConcatStream does not have specified length.", "Expecting cStream1 to raise ArgumentException with message 'First Stream in ConcatStream does not have specified length.'");
			}

			ConcatStream cStream2 = new ConcatStream (mStream1, mStream2, 12);
			Assert.That (cStream2.CanSeek == true, "Expecting cStream2.CanSeek == true");
			Assert.That (cStream2.HasLength == true, "Expecting cStream2.HasLength == true");
			Assert.That (cStream2.CustomLength == true, "Expecting cStream.CustomLength == true;");

			ConcatStream cStream3 = new ConcatStream (mStream1, nStream1, 12);
			Assert.That (cStream3.CanSeek == false, "Expecting cStream2.CanSeek == false");
			Assert.That (cStream3.HasLength == false, "Expecting cStream2.HasLength == false");
			Assert.That (cStream3.CustomLength == true, "Expecting cStream.CustomLength == true;");
		}

		[Test ()] 
		public void CanReadTest ()
		{
			string line = "abcdefg";
			MemoryStream mStream1 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			MemoryStream mStream2 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			NoSeekMemoryStream nStream1 = new NoSeekMemoryStream(Encoding.ASCII.GetBytes (line));

			ConcatStream cStream1 = new ConcatStream (mStream1, mStream2);
			Assert.AreEqual (cStream1.CanRead, (true));

			ConcatStream cStream2 = new ConcatStream (mStream1, nStream1);
			Assert.AreEqual (cStream2.CanRead,(true));

			ConcatStream cStream3 = new ConcatStream (mStream1, mStream2, 12);
			Assert.AreEqual (cStream3.CanRead, (true));

			ConcatStream cStream4 = new ConcatStream (mStream1, nStream1, 12);
			Assert.AreEqual (cStream4.CanRead, (true));
		}

		[Test ()] 
		public void CanWriteTest ()
		{
			string line = "abcdefg";
			MemoryStream mStream1 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			MemoryStream mStream2 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			NoSeekMemoryStream nStream1 = new NoSeekMemoryStream(Encoding.ASCII.GetBytes (line));

			ConcatStream cStream1 = new ConcatStream (mStream1, mStream2);
			Assert.AreEqual (cStream1.CanWrite, (true));

			ConcatStream cStream2 = new ConcatStream (mStream1, nStream1);
			Assert.AreEqual (cStream2.CanWrite, (true));

			ConcatStream cStream3 = new ConcatStream (mStream1, mStream2, 12);
			Assert.AreEqual (cStream3.CanWrite, (true));

			ConcatStream cStream4 = new ConcatStream (mStream1, nStream1, 12);
			Assert.AreEqual (cStream4.CanWrite, (true));
		}

		[Test ()] 
		public void CanSeekTest ()
		{
			string line = "abcdefg";
			MemoryStream mStream1 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			MemoryStream mStream2 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			NoSeekMemoryStream nStream1 = new NoSeekMemoryStream(Encoding.ASCII.GetBytes (line));

			ConcatStream cStream1 = new ConcatStream (mStream1, mStream2);
			Assert.AreEqual (cStream1.CanSeek, (true));

			ConcatStream cStream2 = new ConcatStream (mStream1, nStream1);
			Assert.AreEqual (cStream2.CanSeek, (false));

			ConcatStream cStream3 = new ConcatStream (mStream1, mStream2, 12);
			Assert.AreEqual (cStream3.CanSeek, (true));

			ConcatStream cStream4 = new ConcatStream (mStream1, nStream1, 12);
			Assert.AreEqual (cStream4.CanSeek, (false));
		}

		[Test ()] 
		public void LengthTest ()
		{
			string line = "abcdefg";
			MemoryStream mStream1 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			MemoryStream mStream2 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			NoSeekMemoryStream nStream1 = new NoSeekMemoryStream(Encoding.ASCII.GetBytes (line));

			ConcatStream cStream1 = new ConcatStream (mStream1, mStream2);
			Assert.AreEqual (cStream1.Length, (line.Length * 2));

			ConcatStream cStream2 = new ConcatStream (mStream1, nStream1);
			try {
				long length = cStream2.Length;
			} catch (Exception e) {
				Assert.AreEqual (e.Message, "The instance of ConcatStream does not support Length.");
			}

			ConcatStream cStream3 = new ConcatStream (mStream1, mStream2, 12);
			Assert.AreEqual (cStream3.Length, 12);

			ConcatStream cStream4 = new ConcatStream (mStream1, nStream1, 12);
			Assert.AreEqual (cStream4.Length, 12);
		}

		[Test ()] 
		public void ExpandableTest ()
		{
			string line = "abcdefg";
			MemoryStream mStream1 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			MemoryStream mStream2 = new MemoryStream ();
			NoSeekMemoryStream nStream1 = new NoSeekMemoryStream(Encoding.ASCII.GetBytes (line));

			ConcatStream cStream1 = new ConcatStream (mStream1, mStream2);
			Assert.AreEqual (cStream1.Expandable, (true));

			ConcatStream cStream2 = new ConcatStream (mStream1, nStream1);
			Assert.AreEqual (cStream2.Expandable, (false));

			ConcatStream cStream3 = new ConcatStream (mStream1, mStream2, 12);
			Assert.AreEqual (cStream3.Expandable, (false));

			ConcatStream cStream4 = new ConcatStream (mStream1, nStream1, 12);
			Assert.AreEqual (cStream4.Expandable, (false));
		}
		[Test ()] 
		public void SetLengthTest ()
		{
			string line = "abcdefg";
			MemoryStream mStream1 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			MemoryStream mStream2 = new MemoryStream ();
			NoSeekMemoryStream nStream1 = new NoSeekMemoryStream(Encoding.ASCII.GetBytes (line));

			ConcatStream cStream1 = new ConcatStream (mStream1, mStream2);
			cStream1.SetLength (42);
			Assert.AreEqual (cStream1.Length, (42));

			ConcatStream cStream2 = new ConcatStream (mStream1, nStream1);
			try {
				cStream2.SetLength (42);
			} catch (Exception e) {
				Assert.AreEqual (e.Message, "The instance of ConcatStream does not support SetLength."); 
			}

			ConcatStream cStream3 = new ConcatStream (mStream1, mStream2, 12);
			cStream3.SetLength (42);
			Assert.AreEqual (cStream3.Length, (42));

			ConcatStream cStream4 = new ConcatStream (mStream1, nStream1, 12);
			try {
				cStream4.SetLength (42);
			} catch (Exception e) {
				Assert.AreEqual (e.Message, "The NoSeekMemoryStream does not support seeking"); 
			}
		}

		[Test ()] 
		public void PositionTest ()
		{
			string line = "abcdefg";
			MemoryStream mStream1 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			MemoryStream mStream2 = new MemoryStream ();
			NoSeekMemoryStream nStream1 = new NoSeekMemoryStream(Encoding.ASCII.GetBytes (line));

			ConcatStream cStream1 = new ConcatStream (mStream1, mStream2);
			cStream1.Position = -5;
			Assert.AreEqual (cStream1.Position, (0));
			cStream1.Position = 5;
			Assert.AreEqual (cStream1.Position, (5));

			ConcatStream cStream2 = new ConcatStream (mStream1, nStream1);
			try {
				cStream2.Position = -5;
			} catch (Exception e) {
				Assert.AreEqual (e.Message, "This instance of ConcatStream does not support Seeking."); 
			}

			ConcatStream cStream3 = new ConcatStream (mStream1, mStream2, 12);
			cStream3.Position = -5;
			Assert.AreEqual (cStream3.Position, (0));
			cStream3.Position = 5;
			Assert.AreEqual (cStream3.Position, (5));

			ConcatStream cStream4 = new ConcatStream (mStream1, nStream1, 12);
			try {
				cStream4.Position = -5;
			} catch (Exception e) {
				Assert.AreEqual (e.Message, "This instance of ConcatStream does not support Seeking."); 
			}
		}

		[Test ()] 
		public void SeekTest ()
		{
			string line = "abcdefg";
			MemoryStream mStream1 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			MemoryStream mStream2 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			NoSeekMemoryStream nStream1 = new NoSeekMemoryStream(Encoding.ASCII.GetBytes (line));

			ConcatStream cStream1 = new ConcatStream (mStream1, mStream2);
			cStream1.Seek(5, SeekOrigin.Begin);
			Assert.AreEqual (cStream1.Position, (5));
			cStream1.Seek(5, SeekOrigin.Current);
			Assert.AreEqual (cStream1.Position, (10));
			cStream1.Seek(-4, SeekOrigin.End);
			Assert.AreEqual (cStream1.Position, (10));

			ConcatStream cStream2 = new ConcatStream (mStream1, nStream1);
			try {
				cStream2.Seek(5, SeekOrigin.Begin);
			} catch (Exception e) {
				Assert.AreEqual (e.Message, "The concat stream in this instance does not support seeking."); 
			}
			try {
				cStream2.Seek(-5, SeekOrigin.End);
			} catch (Exception e) {
				Assert.AreEqual (e.Message, "The concat stream in this instance does not support seeking."); 
			}

			ConcatStream cStream3 = new ConcatStream (mStream1, mStream2, 12);
			cStream3.Seek(5, SeekOrigin.Begin);
			Assert.AreEqual (cStream3.Position, (5));
			cStream3.Seek(5, SeekOrigin.Current);
			Assert.AreEqual (cStream3.Position, (10));
			cStream3.Seek(-4, SeekOrigin.End);
			Assert.AreEqual (cStream3.Position, (8));

			ConcatStream cStream4 = new ConcatStream (mStream1, nStream1, 12);
			try {
				cStream4.Seek(5, SeekOrigin.Begin);
			} catch (Exception e) {
				Assert.AreEqual (e.Message, "The concat stream in this instance does not support seeking."); 
			}
			try {
				cStream4.Seek(-5, SeekOrigin.End);
			} catch (Exception e) {
				Assert.AreEqual (e.Message, "The concat stream in this instance does not support seeking."); 
			}
		}

		[Test ()] 
		public void ReadTest ()
		{
			string line = "abcdefg"; 
			string line2 = "ABCDEFG";
			MemoryStream mStream1 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			MemoryStream mStream2 = new MemoryStream (Encoding.ASCII.GetBytes (line2));
			MemoryStream mStream3 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			NoSeekMemoryStream nStream1 = new NoSeekMemoryStream(Encoding.ASCII.GetBytes (line2));

			ConcatStream cStream1 = new ConcatStream (mStream1, mStream2);
			ConcatStream cStream2 = new ConcatStream (mStream3, nStream1, 10);

			byte[] results1 = new byte[1024];
			byte[] results2 = new byte[1024];

			int br1 = cStream1.Read (results1, 0, 50);
			int br2 = cStream2.Read (results2, 0, 50);

			Assert.AreEqual (14, br1);
			Assert.AreEqual (10, br2);

			Assert.AreEqual (line + line2, Encoding.ASCII.GetString (results1, 0, line.Length + line2.Length));
			Assert.AreEqual ("abcdefgABC", Encoding.ASCII.GetString (results2, 0, "abcdefgABC".Length));
		}

		[Test ()] 
		public void WriteTest ()
		{
			string line = "abcdefg"; 
			string line2 = "ABCDEFG";
			string line3 = "123";
			MemoryStream mStream1 = new MemoryStream (Encoding.ASCII.GetBytes (line));
			MemoryStream mStream2 = new MemoryStream (Encoding.ASCII.GetBytes (line2));

			ConcatStream cStream1 = new ConcatStream (mStream1, mStream2);
			cStream1.Seek (3, SeekOrigin.Begin);
			cStream1.Write (Encoding.ASCII.GetBytes (line3), 0, 3);

			byte[] results = new byte[1024];
			cStream1.Seek (0, SeekOrigin.Begin);
			int bytesRead = cStream1.Read (results, 0, 50);

			Assert.AreEqual (14, bytesRead);

			string result = Encoding.ASCII.GetString (results, 0, 14);

			Assert.AreEqual ("abc123gABCDEFG", result);
		}

	}


}

