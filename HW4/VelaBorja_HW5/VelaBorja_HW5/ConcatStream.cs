/****************************************************\
 * 		Nathan VelaBorja
 * 		September 28, 2016
 * 		CptS 422 HW 5
 *		This assignment extends the web server from HW3
 *			to have proper concurrency and the ability
 *			to support web services/apps on top of the
 *			core.
 \***************************************************/

using System;
using System.IO;

namespace CS422
{
	/// <summary>
	/// Represents a concatination of the two given streams. Can be treated as a single stream.
	/// </summary>
	public class ConcatStream : Stream
	{
		private Stream First;						// Local member to represent first stream
		private Stream Second;						// Local member to represent second stream
		private long length;						// Local member to represent the length of both streams combined
		private long position;						// Cross-stream position

		public ConcatStream (Stream first, Stream second)
		{
			First = first;							// Save streams
			Second = second;
			length = -1;							// Store length as negative to let Property know it must query first/second stream for length
			position = 0;							// Start position at 0

			// Make sure first stream has a set length, otherwise this class is useless and must throw an exception
			try {
				long testLength = First.Length;
				testLength += 1;
			} catch (Exception ex) {
				throw new ArgumentException ("First Stream in ConcatStream does not have specified length.");					
			}
		}

		public ConcatStream (Stream first, Stream second, long fixedLength)
		{
			First = first;							// Save streams
			Second = second;
			length = fixedLength;					// Let Property know that we will return this value for length, not query streams
			position = 0;							// Start position at 0
		}

		public override bool CanRead {
			get {
				return First.CanRead && Second.CanRead;
			}
		}

		public override bool CanSeek {
			get {
				return First.CanSeek && Second.CanSeek;
			}
		}

		public override bool CanWrite {
			get {
				return First.CanWrite && Second.CanWrite;
			}
		}

		public override long Length {
			get {
				if (length > -1) {						// If length was specified in constructor, return that value
					return length;
				}

				return First.Length + Second.Length;	// Otherwise query the streams from length and return that
			}
		}

		public override void SetLength (long value)
		{
			if (length > -1) {							// If length was specified in constructor, let it be changed directly
				length = value;
				return;
			}

			// If length wasn't specified by the constructor, there are a few cases we have to worry about
			if (value > First.Length) {					// If the value goes into the second stream
				Second.SetLength(value - First.Length);	// Forward to the second stream
				return;
			}

			if (value <= First.Length) {				// If the value goes into the first stream
				Second.SetLength (0);					// Set length of second stream to 0
				First.SetLength (value);				// Set length of first stream to value
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

				// Set relative positions for first and second stream
				if (position < First.Length) {
					First.Position = position;
					Second.Position = 0;
				} else {
					First.Position = First.Length - 1;
					Second.Position = position - First.Length;
				}
			}
		}

		public override void Flush ()
		{
			return;
		}

		public override long Seek (long offset, SeekOrigin origin)
		{
			if (!CanSeek)
				return 0;

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

			// Set relative positions for first and second stream
			if (Position < First.Length) {
				First.Position = Position;
				Second.Position = 0;
			} else {
				First.Position = First.Length - 1;
				Second.Position = Position - First.Length;
			}

			return Position;
		}

		public override int Read (byte[] buffer, int offset, int count)
		{
			if (!CanRead)
				return 0;

			int bytesRead = First.Read (buffer, offset, count);

			if (bytesRead < count) {
				bytesRead += Second.Read (buffer, offset + bytesRead, count - bytesRead);
			}

			return bytesRead;
		}

		public override void Write (byte[] buffer, int offset, int count)
		{
			if (!CanWrite)
				return 0;
			
			int bytesWritten = First.Write (buffer, offset, count);

			if (bytesWritten < count) {
				bytesWritten += Second.Read (buffer, offset + bytesWritten, count - bytesWritten);
			}

			return bytesWritten;
		}
	}
}

