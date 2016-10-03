using NUnit.Framework;
using System;
using System.IO;
using CS422;

namespace HW5_UnitTests
{
	[TestFixture ()]
	public class Test
	{
		/*[Test ()] 
		public void NoSeekMemoryStreamConstructorTest ()	// Not currently working, can't figure out a clean way to test results with a byte buffer
		{
			string upperString = "ALL CHARS IN THIS STRING ARE UPPER CASE";
			string lowerString = "all chars in this string are lower case";

			byte[] upperBytes = System.Text.Encoding.Unicode.GetBytes (upperString);
			byte[] lowerBytes = System.Text.Encoding.Unicode.GetBytes (lowerString);

			byte[] results1 = new byte[1024];
			byte[] results2 = new byte[1024];
			byte[] expected1 = System.Text.Encoding.Unicode.GetBytes ("ALL CHARS IN THIS STRING ARE UPPER CASE");
			byte[] expected2 = System.Text.Encoding.Unicode.GetBytes ("chars in t");

			NoSeekMemoryStream upper1 = new NoSeekMemoryStream (upperBytes);
			NoSeekMemoryStream upper2 = new NoSeekMemoryStream (lowerBytes, 4, 10);

			upper1.Read (results1, 0, 1024);
			upper2.Read (results2, 0, 1024);

			Assert.That (System.Text.Encoding.Unicode.GetString(results1).Trim('\0'), Is.EqualTo (System.Text.Encoding.Unicode.GetString(expected1).Trim('\0')));
			Assert.That (System.Text.Encoding.Unicode.GetString(results2).Trim('\0'), Is.EqualTo (System.Text.Encoding.Unicode.GetString(expected2).Trim('\0')));
		}*/

	}


}

