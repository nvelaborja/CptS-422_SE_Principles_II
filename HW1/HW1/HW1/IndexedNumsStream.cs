/**********************************************\
 * Author: Nathan VelaBorja
 * Project: CptS 422 HW1
 * Date: August 30, 2016
 * ********************************************/

using System;
using System.IO;

namespace CS422
{
	/// <summary>
	/// Read-only stream of procedurally-generated indexed data. Value = Position % 256. 
	/// </summary>
	public class IndexedNumsStream : System.IO.Stream
	{
		private long position;
		private long length;

		/// <summary>
		/// Constructs a new IndexedNumsStream of the given length
		/// </summary>
		/// <param name="Length">Length.</param>
		public IndexedNumsStream (long Length)
		{
			length = Length;
			position = 0;
		}

		public override bool CanRead {
			get {
				return true;
			}
		}

		public override bool CanSeek {
			get {
				return true;
			}
		}

		public override bool CanWrite {
			get {
				return false;
			}
		}

		public override long Length {
			get {
				return length;
			}
		}

		public override long Position {
			get {
				return position;
			}
			set {
				if (value < 0)
					value = 0;
				if (value > Length)
					value = Length;
				position = value;
			}
		}

		public override void Flush ()
		{
			return;
		}
			
		public override long Seek (long offset, SeekOrigin origin)
		{
			Position = offset;

			switch (origin) {
			case SeekOrigin.Begin:
				Position = offset;
				break;
			case SeekOrigin.Current:
				Position = Position + offset;
				break;
			case SeekOrigin.End:
				Position = Length - 1 + offset;
				break;
			}

			return Position;
		}

		public override void SetLength (long value)
		{
			if (value < 0)
				value = 0;
			length = value;
		}

		/// <summary>
		/// Reads count number of bytes from stream to buffer beginning at offset.
		/// </summary>
		/// <param name="buffer">Buffer.</param>
		/// <param name="offset">Offset.</param>
		/// <param name="count">Count.</param>
		public override int Read (byte[] buffer, int offset, int count)
		{
			int bytesWritten = 0;

			for (int i = 0; i < count; i++) {
				if (Position > Length - 1)						// If we reach the end of the stream, return the bytesWritten up to that point
					return bytesWritten;

				buffer [offset + i] = (byte)(Position++ % 256);	// Otherwise get the next value from the stream, write it to the buffer, and increment position

				bytesWritten++;									// Increment bytes read after successful read
			}

			return bytesWritten;
		}

		public override void Write (byte[] buffer, int offset, int count)
		{
			return;
		}
	}
}
