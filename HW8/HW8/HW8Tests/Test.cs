/***********************************\
 * Nathan VelaBorja
 * October 21, 2016
 * Cpts 422 HW 8
 * Collaborators: 
 * 	None, but helped a few others
 * 	with some logic.
\***********************************/

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using CS422;

namespace HW8Tests
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void StdFileSysContainsDirTest ()
		{
			StandardFileSystem fs = StandardFileSystem.Create ("/home/nathan/Documents");
			bool a = fs.Contains (new StdFSDir ("/home/nathan/Documents/CptS-422_SE_Principles_II"));
			bool b = fs.Contains (new StdFSDir ("/home/nathan/Documents/CptS-422_SE_Principles_II/HW8"));
			bool c = fs.Contains (new StdFSDir ("/home/nathan/Documents/CptS-422_SE_Principles_II/HW8/HW8"));
			bool d = fs.Contains (new StdFSDir ("/home/nathan/Documents/CptS-422_SE_Principles_II/HW8/EventPractice"));

			Assert.That (a);
			Assert.That (b);
			Assert.That (c);
			Assert.That (d);
		}

		public void StdFileSysContainsFileTest ()
		{
			StandardFileSystem fs = StandardFileSystem.Create ("/home/nathan/Documents");
			bool a = fs.Contains (new StdFSFile ("/home/nathan/Documents/CptS-422_SE_Principles_II/HW8/assignment.pdf"));
			bool b = fs.Contains (new StdFSFile ("/home/nathan/Documents/CptS-422_SE_Principles_II/HW8/FileSystemDefs.cs"));

			Assert.That (a);
			Assert.That (b);
		}

		[Test()]
		public void StdFileTest ()
		{
			StandardFileSystem fs = StandardFileSystem.Create ("/home/nathan/Documents/CptS-422_SE_Principles_II/HW8");
			Dir422 root = fs.GetRoot ();
			root.CreateFile ("TestFile1");
			Assert.That (root.ContainsFile ("TestFile1", false));

			StandardFileSystem fs2 = StandardFileSystem.Create ("/home/nathan/Documents/");
			Dir422 root2 = fs2.GetRoot ();
			Assert.That (root2.ContainsFile ("TestFile1", true));
		}

		[Test()]
		public void StdDirTest ()
		{
			StandardFileSystem fs = StandardFileSystem.Create ("/home/nathan/Documents/CptS-422_SE_Principles_II/HW8");
			Dir422 root = fs.GetRoot ();
			root.CreateDir ("TestDir1");
			Assert.That (root.ContainsDir ("TestDir1", false));

			StandardFileSystem fs2 = StandardFileSystem.Create ("/home/nathan/Documents/");
			Dir422 root2 = fs2.GetRoot ();
			Assert.That (root2.ContainsDir ("TestDir1", true));
		}

		[Test()]
		public void MemFileSysTest ()
		{
			MemoryFileSystem fs = new MemoryFileSystem ();
			Dir422 root = fs.GetRoot ();
			root.CreateDir ("Depth1Dir");
			root.CreateFile ("Depth1File");

			bool a = (root.ContainsDir ("Depth1Dir", false));
			bool b = (root.ContainsFile ("Depth1File", false));

			Assert.That (a);
			Assert.That (b);
		}

		[Test()]
		public void MemFileTest ()
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

			Assert.That (System.Text.Encoding.ASCII.GetString (buffer) == "This is a test");
		}

		[Test()]
		public void MemDirTest ()
		{
			MemoryFileSystem fs = new MemoryFileSystem ();
			Dir422 root = fs.GetRoot ();
			root.CreateDir ("Depth1Dir");
			root.CreateFile ("Depth1File");

			Dir422 c = root.GetDir ("Depth1Dir");
			File422 d = root.GetFile ("Depth1File");

			Assert.That (c.Name == "Depth1Dir");
			Assert.That (d.Name == "Depth1File");
		}
	}
}

