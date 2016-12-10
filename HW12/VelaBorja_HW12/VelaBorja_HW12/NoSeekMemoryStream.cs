using System;
using System.IO;

namespace CS422
{
	public class NoSeekMemoryStream : MemoryStream
	{
		private MemoryStream stream;

		public NoSeekMemoryStream(byte[] buffer)
		{
			stream = new MemoryStream (buffer);
		}

		public NoSeekMemoryStream(byte[] buffer, int offset, int count)
		{
			stream = new MemoryStream (buffer, offset, count);
		}

		public override bool CanRead {
			get {
				return stream.CanRead;
			}
		}

		public override bool CanSeek {
			get {
				return false;
			}
		}

		public override bool CanWrite {
			get {
				return stream.CanWrite;
			}
		}

		public override long Length {
			get {
				throw new NotSupportedException ("The NoSeekMemoryStream does not support seeking");
			}
		}

		public override void SetLength (long value)
		{
			throw new NotSupportedException ("The NoSeekMemoryStream does not support seeking");
		}

		public override long Position {
			get {
				throw new NotSupportedException ("The NoSeekMemoryStream does not support seeking");
			}
			set {
				throw new NotSupportedException ("The NoSeekMemoryStream does not support seeking");
			}
		}

		public override void Flush ()
		{
			stream.Flush ();
			return;
		}

		public override long Seek (long offset, SeekOrigin origin)
		{
			throw new NotSupportedException ("The NoSeekMemoryStream does not support Seek()");
		}

		public override int Read (byte[] buffer, int offset, int count)
		{
			return stream.Read (buffer, offset, count);
		}

		public override void Write (byte[] buffer, int offset, int count)
		{
			stream.Write (buffer, offset, count);

			return;
		}
	}
}

