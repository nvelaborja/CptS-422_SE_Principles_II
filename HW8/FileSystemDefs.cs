using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS422
{
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

		public abstract Stream OpenReadOnly();

		public abstract Stream OpenReadWrite();
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

	public class StdFSDir : Dir422
	{
		private string m_path;

		public StdFSDir(string path)
		{
			m_path = path;
		}

		public override IList<File422> GetFiles()
		{
			List<File422> files = new List<File422>();

			foreach (string file in Directory.GetFiles(m_path))
			{
				files.Add(new StdFSFile(file));
			}

			return files;
		}
	}

	public class StdFSFile : File422
	{
		private string m_path;

		public StdFSFile(string path) 
		{
			m_path = path;
		}

		public Stream OpenReadOnly()
		{

		}
	}

}


// For memFile
	// Override MemStream as ObservableMemStream that only overrides Displose(). In dispose, call an event that will let FileSys know the stream has closed