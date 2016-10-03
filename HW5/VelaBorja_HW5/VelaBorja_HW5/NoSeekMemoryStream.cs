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
			SetLength (data.Length);
		}

		public NoSeekMemoryStream(byte[] buffer, int offset, int count)
		{
			data = buffer;
			position = offset;
			SetLength (count);
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
				return length;
			}
		}

		public override void SetLength (long value)
		{
			if (value > data.Length)
				value = data.Length;

			length = value;
		}

		public override long Position {
			get {
				return position;
			}
			set {
				if (value < 0)
					value = 0;
				if (value > length - 1)
					value = length;
				position = value;
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
				if (position > Length - 1 || (offset + i) > buffer.Length - 1)			// If we're beyond the stream or beyond the bounds of the buffer
					return bytesRead;
				
				buffer [offset + i] = data [Position];

				Position += 1;
				bytesRead++;

				i++;
			}

			return bytesRead;
		}

		public override void Write (byte[] buffer, int offset, int count)
		{
			for (int i = 0; i < count; i++) {
				if (position > Length - 1)
					return;
				
				data [Position] = buffer [offset + i];
				Position++;
			}

			return;
		}
	}
}

