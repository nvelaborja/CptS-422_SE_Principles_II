using System;
using System.IO;

namespace CS422
{
	public class NoSeekMemoryStream : MemoryStream
	{
		private byte[] data;
		private long position;
		private long length;

		public NoSeekMemoryStream(byte[] buffer)
		{
			data = buffer;
			position = 0;
			length = buffer.Length;
		}

		public NoSeekMemoryStream(byte[] buffer, int offset, int count)
		{
			data = buffer;
			position = offset;
			length = count;
		}

		public override bool CanRead {
			get {
				return true;
			}
		}

		public override bool CanSeek {
			get {
				return false;
			}
		}

		public override bool CanWrite {
			get {
				return true;
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

			return;
		}

		public override long Seek (long offset, SeekOrigin origin)
		{
			throw new NotSupportedException ("The NoSeekMemoryStream does not support Seek()");
		}

		public override int Read (byte[] buffer, int offset, int count)
		{
			int bytesRead = 0, i = 0;

			while (i < count) {
				if (position > length - 1 || (offset + i) > buffer.Length - 1)			// If we're beyond the stream or beyond the bounds of the buffer
					return bytesRead;
				
				buffer [offset + i] = data [position];

				position += 1;
				bytesRead++;

				i++;
			}

			return bytesRead;
		}

		public override void Write (byte[] buffer, int offset, int count)
		{
			for (int i = 0; i < count; i++) {
				if (position > length - 1 || (offset + i) > buffer.Length - 1)			// If we're beyond the stream or beyond the bounds of the buffer
					return;
				
				data [position] = buffer [offset + i];
				position++;
			}

			return;
		}
	}
}

