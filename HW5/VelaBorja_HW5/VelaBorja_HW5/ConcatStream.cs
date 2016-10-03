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
			Position = 0;							// Start position at 0

			// Make sure first stream has a set length, otherwise this class is useless and must throw an exception
			try {
				long testLength = First.Length;
				testLength += 1;
			} catch {
				throw new ArgumentException ("First Stream in ConcatStream does not have specified length.");					
			}
		}

		public ConcatStream (Stream first, Stream second, long fixedLength)
		{
			First = first;							// Save streams
			Second = second;
			length = fixedLength;					// Let Property know that we will return this value for length, not query streams
			Position = 0;							// Start position at 0
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
					Second.Position = position - First.Length;	// -1?
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

			switch (origin) {
				case SeekOrigin.Begin:
					Position = offset;
					break;
				case SeekOrigin.Current:
					Position = Position + offset;
					break;
				case SeekOrigin.End:
					if (length < 0) {			// If length was specified in constructor, we can do this
						Position = Length + offset;
					} else
						throw new ArgumentException ("Cannot Seek from SeekOrigin.End");
					break;
				default:
					throw new ArgumentException ("Error with SeekOrigin");
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

			Position += bytesRead;

			return bytesRead;
		}

		public override void Write (byte[] buffer, int offset, int count)
		{
			// 		0	1	2	3	4	5	6	7	8	9
			//						P = 4							Length = 10;
			// 		Room Left = Length - Position = 10 - 4 =  6

			if (!CanWrite)
				return 0;
			

			// First try writing to the first stream
			int roomLeft = First.Length - First.Position;			// Record how much is expected to be written to first stream, but really just amount of room left in first stream
			First.Write (buffer, offset, count);					// Write to first stream, stream will stop writing when runs out of room

			// Then write to the second stream if there is any writing left to be done
			if (roomLeft < count) {
				Second.Write (buffer, offset + roomLeft, count - roomLeft);
			}

			// We don't know how much was actually written to the streams, so we need to reset position based on streams
			Position = First.Position + Second.Position;
		}
	}
}

