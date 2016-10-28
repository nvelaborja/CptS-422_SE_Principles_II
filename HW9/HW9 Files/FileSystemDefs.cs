/***********************************\
 * Nathan VelaBorja
 * October 24, 2016
 * Cpts 422 HW 9
 * Collaborators: 
 * 	None
\***********************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CS422
{
	#region Base Filesystem Classes

	public abstract class Dir422
	{
		public abstract string Name { get; }

		public abstract IList<Dir422> GetDirs();

		public abstract IList<File422> GetFiles();

		public abstract Dir422 Parent { get; }

		public abstract bool ContainsFile(string fileName, bool recursive);

		public abstract bool ContainsDir(string dirName, bool recursive);

		public abstract Dir422 GetDir(string name);

		public abstract File422 GetFile(string name);

		public abstract File422 CreateFile(string name);

		public abstract Dir422 CreateDir(string name);
	}

	public abstract class File422
	{
		public abstract string Name { get; }

		public abstract Dir422 Parent { get; }

		public abstract string FileType { get; }

		public abstract Stream OpenReadOnly();

		public abstract Stream OpenReadWrite ();

		// public abstract DateTime Created { get; }
		// public abstract DateTime Modified { get; }
	}

	public abstract class FileSys422
	{
		public abstract Dir422 GetRoot();

		public virtual bool Contains(File422 file)
		{
			return Contains(file.Parent);
		}
		
		public virtual bool Contains(Dir422 dir)
		{
			if (dir == null)
				return false;

			if (dir == GetRoot())
				return true;

			return Contains(dir.Parent);
		}
	}

	#endregion

	#region Standard File System Classes

	public class StdFSDir : Dir422
	{
		private string m_path;
		private string m_name;

		public StdFSDir(string path)
		{
			m_path = path;
			m_name = System.IO.Path.GetFileName (m_path);
		}

		public static bool operator ==(StdFSDir dir1, StdFSDir dir2)
		{
			return dir1.m_name == dir2.m_name;
		}

		public static bool operator !=(StdFSDir dir1, StdFSDir dir2)
		{
			return dir1.m_path != dir2.m_path;
		}

		public override string Name {
			get {
				return m_name;
			}
		}

		public string Path {
			get {
				return m_path;
			}
		}

		public override IList<Dir422> GetDirs()
		{
			List<Dir422> dirs = new List<Dir422> ();

			foreach (string dir in Directory.GetDirectories(m_path)) {
				dirs.Add (new StdFSDir (dir));
			}

			return dirs;
		}

		public override IList<File422> GetFiles()
		{
			List<File422> returnFiles = new List<File422>();
			string[] localFiles;

			try {
				localFiles = Directory.GetFiles (m_path);
			} catch (Exception ex) {
				return new List<File422> ();
			}


			foreach (string file in localFiles)
			{
				returnFiles.Add(new StdFSFile(file));
			}


			return returnFiles;
		}

		public override Dir422 Parent 
		{ 
			get{
				// Will be null if root
				DirectoryInfo parentInfo = Directory.GetParent (m_path);

				if (parentInfo == null)
					return null;

				return new StdFSDir(parentInfo.FullName);
			}
		}

		public override bool ContainsFile(string fileName, bool recursive)
		{
			List<File422> files;

			if (fileName.Contains ("/") || fileName.Contains ("\\"))
				return false;

			if (recursive) {
				// First check this directory
				files = (List<File422>)GetFiles ();

				foreach (File422 file in files) {
					if (file.Name == fileName)
						return true;
				}

				// If need be, go deep T-T (Depth First Search)
				List<Dir422> dirs = (List<Dir422>)GetDirs ();
				foreach (Dir422 dir in dirs) {
					if (dir.ContainsFile (fileName, true))
						return true;
				}

				return false;
			}

			// Not recursive, don't go any deeper than current directory
			files = (List<File422>)GetFiles ();

			foreach (File422 file in files) {
				if (file.Name == fileName)
					return true;
			}

			return false;
		}

		public override bool ContainsDir(string dirName, bool recursive)
		{
			List<Dir422> dirs;

			if (dirName.Contains ("/") || dirName.Contains ("\\"))
				return false;

			if (recursive) {
				// First check this directory
				dirs = (List<Dir422>)GetDirs ();

				foreach (Dir422 dir in dirs) {
					if (dir.Name == dirName)
						return true;
				}

				// If need be, go deep T-T (Depth First Search)
				foreach (Dir422 dir in dirs) {
					if (dir.ContainsDir (dirName, true))
						return true;
				}

				return false;
			}

			// Not recursive, don't go any deeper than current directory
			dirs = (List<Dir422>)GetDirs ();

			foreach (Dir422 dir in dirs) {
				if (dir.Name == dirName)
					return true;
			}

			return false;
		}

		public override Dir422 GetDir(string name)
		{
			if (name.Contains ("/") || name.Contains ("\\"))
				return null;

			if (ContainsDir(name, false))
				return new StdFSDir (string.Format ("{0}/{1}", m_path, name));
			return null;
		}

		public override File422 GetFile(string name)
		{
			if (name.Contains ("/") || name.Contains ("\\"))
				return null;

			if (ContainsFile(name, false))
				return new StdFSFile(string.Format ("{0}/{1}", m_path, name));
			return null;
		}

		public override File422 CreateFile(string name)
		{
			if (name.Contains ("/") || name.Contains ("\\"))
				return null;
			File.Create (string.Format ("{0}/{1}", m_path, name));
			return new StdFSFile (string.Format ("{0}/{1}", m_path, name));
		}

		public override Dir422 CreateDir(string name)
		{
			if (name.Contains ("/") || name.Contains ("\\"))
				return null;
			Directory.CreateDirectory (string.Format ("{0}/{1}", m_path, name));
			return new StdFSDir (string.Format ("{0}/{1}", m_path, name));
		}
	}

	public class StdFSFile : File422
	{
		private string m_path;
		private string m_name;

		public StdFSFile(string path) 
		{
			m_path = path;
			m_name = Path.GetFileName (path);
		}

		public override string Name {
			get {
				return m_name;
			}
		}

		public override Dir422 Parent 
		{ 
			get{
				// Will be null if root
				return new StdFSDir(Directory.GetParent (m_path).FullName);
			}
		}

		public override string FileType {
			get {
				return Path.GetExtension (m_path).Replace (".", "");
			}
		}

		public override Stream OpenReadOnly()
		{
			return File.Open (m_path, FileMode.Open, FileAccess.Read);
		}

		public override Stream OpenReadWrite ()
		{
			return File.Open (m_path, FileMode.Open, FileAccess.ReadWrite);
		}
	}

	public class StandardFileSystem : FileSys422
	{
		private static StdFSDir m_root;

		public override Dir422 GetRoot()
		{
			return m_root;
		}

		public static StandardFileSystem Create (string rootDir)
		{
			m_root = new StdFSDir (rootDir);
			return new StandardFileSystem ();
		}

		public override bool Contains(File422 file)
		{
			return Contains(file.Parent) && file.Parent.ContainsFile(file.Name, false);
		}

		public override bool Contains(Dir422 dir)
		{
			if (dir == null)
				return false;

			StdFSDir dirCopy = (StdFSDir)dir;
			StdFSDir root = (StdFSDir)GetRoot ();

			if (dirCopy == root)
				return true;

			return Contains(dir.Parent);
		}

	}

	#endregion

	#region Memory File System Classes

	/// <summary>
	/// Represents a file system held entirely in memory
	/// </summary>
	public class MemoryFileSystem : FileSys422
	{
		MemFSDir m_root;

		public MemoryFileSystem()
		{
			//Create empty memory file system
			m_root = new MemFSDir ("", null);
		}

		public override Dir422 GetRoot ()
		{
			return m_root;
		}

		public override bool Contains(File422 file)
		{
			return Contains(file.Parent);
		}

		public override bool Contains(Dir422 dir)
		{
			if (dir == null)
				return false;

			if (Object.ReferenceEquals(dir, GetRoot()))
				return true;

			return Contains(dir.Parent);
		}
	}

	public class MemFSDir : Dir422
	{
		private string m_name;
		private Dir422 m_parent;
		private List<Dir422> m_directories;
		private List<File422> m_files;

		public MemFSDir(string name, Dir422 parent)
		{
			m_name = name;
			m_parent = parent;
			m_directories = new List<Dir422> ();
			m_files = new List<File422> ();
		}

		public override string Name {
			get {
				return m_name;
			}
		}

		public override IList<Dir422> GetDirs()
		{
			return m_directories;
		}

		public override IList<File422> GetFiles()
		{
			return m_files;
		}

		public override Dir422 Parent 
		{ 
			get{
				// Will be null if root
				return m_parent;
			}
		}

		public override bool ContainsFile(string fileName, bool recursive)
		{
			List<File422> files;

			if (fileName.Contains ("/") || fileName.Contains ("\\"))
				return false;

			if (recursive) {
				// First check this directory
				files = (List<File422>)GetFiles ();

				foreach (File422 file in files) {
					if (file.Name == fileName)
						return true;
				}

				// If need be, go deep T-T (Depth First Search)
				List<Dir422> dirs = (List<Dir422>)GetDirs ();
				foreach (Dir422 dir in dirs) {
					if (dir.ContainsFile (fileName, true))
						return true;
				}

				return false;
			}

			// Not recursive, don't go any deeper than current directory
			files = (List<File422>)GetFiles ();

			foreach (File422 file in files) {
				if (file.Name == fileName)
					return true;
			}

			return false;
		}

		public override bool ContainsDir(string dirName, bool recursive)
		{
			List<Dir422> dirs;

			if (dirName.Contains ("/") || dirName.Contains ("\\"))
				return false;

			if (recursive) {
				// First check this directory
				dirs = (List<Dir422>) GetDirs ();

				foreach (Dir422 dir in dirs) {
					if (dir.Name == dirName)
						return true;
				}

				// If need be, go deep T-T (Depth First Search)
				foreach (Dir422 dir in dirs) {
					if (dir.ContainsDir (dirName, true))
						return true;
				}

				return false;
			}

			// Not recursive, don't go any deeper than current directory
			dirs = (List<Dir422>) GetDirs ();

			foreach (Dir422 dir in dirs) {
				if (dir.Name == dirName)
					return true;
			}

			return false;
		}

		public override Dir422 GetDir(string name)
		{
			if (name.Contains ("/") || name.Contains ("\\"))
				return null;

			foreach (Dir422 dir in m_directories) {
				if (dir.Name == name)
					return dir;
			}
			return null;
		}

		public override File422 GetFile(string name)
		{
			if (name.Contains ("/") || name.Contains ("\\"))
				return null;

			foreach (File422 file in m_files) {
				if (file.Name == name)
					return file;
			}
			return null;
		}

		public override File422 CreateFile(string name)
		{
			if (name.Contains ("/") || name.Contains ("\\"))
				return null;

			File422 newFile = new MemFSFile (name, this);
			m_files.Add (newFile);

			return newFile;
		}

		public override Dir422 CreateDir(string name)
		{
			if (name.Contains ("/") || name.Contains ("\\"))
				return null;

			Dir422 newDir = new MemFSDir (name, this);
			m_directories.Add (newDir);

			return newDir;
		}
	}

	public class MemFSFile : File422
	{
		private string m_name;
		private Dir422 m_parent;
		private static int m_nwriters;
		private static int m_nreaders;
		private byte[] m_data;				// Each file gets 10Kb of data space. Will need to make this dynamic in the future

		public MemFSFile(string name, Dir422 parent) 
		{
			m_name = name;
			m_parent = parent;
			m_data = new byte[1024 * 10];	
			m_nwriters = 0;
			m_nreaders = 0;
		}

		public override string Name {
			get {
				return m_name;
			}
		}

		public override Dir422 Parent 
		{ 
			get{
				// Will be null if root
				return m_parent;
			}
		}

		public override string FileType {
			get {
				string[] tokens = m_name.Split (new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

				if (tokens.Length > 1)
					return tokens.Last();

				return "";
			}
		}

		/*
		If a stream is open for writing (returned from the OpenReadWrite function), then all calls to
			OpenReadWrite and OpenReadOnly return null, until that stream gets closed/disposed.
		If no streams are open for writing, then each and every call to OpenReadOnly should succeed and
			return a new stream that provides read access to the file data.
		OpenReadWrite must fail if there are any streams open already for the file, be they read-only or
			read/write.
		*/
	
		public override Stream OpenReadOnly()
		{
			lock (m_data) {
				if (m_nwriters > 0)
					return null;

				MemoryStream baseStream = new MemoryStream (m_data, false);
				NotifyDisposeMemoryStream rStream = new NotifyDisposeMemoryStream (baseStream);

				rStream.streamDisposed += DisposeStream;

				m_nreaders++;

				return rStream;
			}
		}

		public override Stream OpenReadWrite ()
		{
			lock (m_data) {
				if (m_nwriters > 0 || m_nreaders > 0)
					return null;

				MemoryStream baseStream = new MemoryStream (m_data, true);
				NotifyDisposeMemoryStream rwStream = new NotifyDisposeMemoryStream (baseStream);

				(rwStream as NotifyDisposeMemoryStream).streamDisposed += DisposeStream;

				m_nwriters++;
				m_nreaders++;

				return rwStream;
			}
		}

		private static void DisposeStream(object sender, EventArgs e)
		{
			// Regardless of the stream type, we will always decrement nreaders;
			m_nreaders--;

			// Check to see if the stream could write
			if ((sender as MemoryStream).CanWrite)
				m_nwriters--;
		}
	}

	public class NotifyDisposeMemoryStream : MemoryStream
	{
		private MemoryStream m_stream;
		public event EventHandler streamDisposed;

		public NotifyDisposeMemoryStream(MemoryStream stream)
		{
			m_stream = stream;
		}

		#region Forwarding Overrides

		public override bool CanRead {
			get {
				return m_stream.CanRead;
			}
		}

		public override bool CanSeek {
			get {
				return m_stream.CanSeek;
			}
		}

		public override bool CanWrite {
			get {
				return m_stream.CanWrite;
			}
		}

		public override int Capacity {
			get {
				return m_stream.Capacity;
			}
			set {
				m_stream.Capacity = value;
			}
		}

		public override bool CanTimeout {
			get {
				return m_stream.CanTimeout;
			}
		}

		public override long Length {
			get {
				return m_stream.Length;
			}
		}

		public override long Position {
			get {
				return m_stream.Position;
			}
			set {
				m_stream.Position = value;
			}
		}

		public override int WriteTimeout {
			get {
				return m_stream.WriteTimeout;
			}
			set {
				m_stream.WriteTimeout = value;
			}
		}

		public override void SetLength (long value)
		{
			m_stream.SetLength (value);
		}

		public override int Read (byte[] buffer, int offset, int count)
		{
			return m_stream.Read (buffer, offset, count);
		}

		public override void Write (byte[] buffer, int offset, int count)
		{
			m_stream.Write (buffer, offset, count);
		}

		public override long Seek (long offset, SeekOrigin loc)
		{
			return m_stream.Seek (offset, loc);
		}

		public override void WriteByte (byte value)
		{
			m_stream.WriteByte (value);
		}

		#endregion 

		protected override void Dispose (bool disposing)
		{
			EventArgs e = new EventArgs ();
			OnStreamDisposed (e);

			m_stream.Dispose ();
		}

		protected virtual void OnStreamDisposed(EventArgs e)
		{
			EventHandler handler = streamDisposed;

			if (handler == null)
				return;

			handler(m_stream, e);
		}
	}

	#endregion

}


// For memFile
	// Override MemStream as ObservableMemStream that only overrides Displose(). In dispose, call an event that will let FileSys know the stream has closed