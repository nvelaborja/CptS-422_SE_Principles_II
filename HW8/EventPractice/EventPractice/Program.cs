using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace EventPractice
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			byte[] buffer = System.Text.Encoding.ASCII.GetBytes ("This is a test string!");

			MemoryStream baseStream = new MemoryStream (buffer);

			Console.WriteLine ("Intial Data:");
			Console.WriteLine (System.Text.Encoding.ASCII.GetString (buffer));

			NotifyDisposeMemoryStream notifyStream = new NotifyDisposeMemoryStream (baseStream);

			notifyStream.streamDisposed += DoSomething;

			byte[] buffer2 = new byte[1024];
			notifyStream.Read (buffer2, 0, 1024);
			Console.WriteLine ("Data from Read:");
			Console.WriteLine (System.Text.Encoding.ASCII.GetString (buffer2));

			notifyStream.Dispose ();

		}

		public static void DoSomething (object sender, EventArgs e)
		{
			Console.WriteLine ("The Event Worked!");
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
			Console.WriteLine ("Firing OnDispose Event!");
			m_stream.Close ();

			EventArgs e = new EventArgs ();
			OnStreamDisposed (e);
		}

		protected virtual void OnStreamDisposed(EventArgs e)
		{
			EventHandler handler = streamDisposed;

			if (handler == null)
				return;

			handler(this, e);
		}
	}
}
