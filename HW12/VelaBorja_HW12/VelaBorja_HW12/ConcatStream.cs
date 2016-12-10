/****************************************************\
 * 		Nathan VelaBorja
 * 		September 28, 2016
 * 		CptS 422 HW 6
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
		private bool hasLength;						// Flag determining whether the second stream supports use of length
		private bool customLength;					// Flag determining whether a custom length was given in constructor

		public ConcatStream (Stream first, Stream second)
		{
			First = first;							// Save streams
			Second = second;
			hasLength = false;						// Default length use to false
			customLength = false;					// This constructor can't declare a custom length
			position = 0;

			// Make sure first stream has a set length, otherwise this class is useless and must throw an exception
			try {
				long testLength = First.Length;
				length = testLength;
			} catch {
				throw new ArgumentException ("First Stream in ConcatStream does not have specified length.");					
			}

			First.Position = 0;

			// Now see if the second stream supports length to determine if the ConcatStream will
			try {
				long testLength = Second.Length;
				length += testLength;
				Second.Position = 0;
			} catch {
				return;								// If it doesn't support length, leave hasLength false and return
			}

			hasLength = true;						// If it does support, set hasLength to true and return;
		}

		public ConcatStream (Stream first, Stream second, long fixedLength)
		{
			First = first;							// Save streams
			Second = second;
			length = fixedLength;					// Let Property know that we will return this value for length, not query streams
			hasLength = false;
			customLength = true;
			position = 0;

			// Make sure first stream has a set length, otherwise this class is useless and must throw an exception
			try {
				long testLength = First.Length;
			} catch {
				throw new ArgumentException ("First Stream in ConcatStream does not have specified length.");					
			}

			// Now see if the second stream supports length to determine if the ConcatStream will
			try {
				long testLength = Second.Length;
			} catch {
				return;								// If it doesn't support length, leave hasLength false and return
			}

			hasLength = true;						// If it does support, set hasLength to true and return;
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
				if (customLength) {							// If a custom length was specified in constructor, return that value
					return length;
				}
				if (hasLength) {
					return First.Length + Second.Length;	// If the second stream has length, we can query for total length
				}

				throw new ArgumentException ("The instance of ConcatStream does not support Length.");
			}
		}

		public bool Expandable {
			get {
				if (customLength || !hasLength)			// Stream cannot be expandable if second stream has no length or a custom length was specified
					return false;

				try {									// If this set length expansion fails, it's not expanable
					long oldLength = Second.Length;
					Second.SetLength(oldLength + 1);	// Try expanding length
					Second.SetLength(oldLength);		// Reset length
				} catch {
					return false;
				}

				return true;							// If it made it through the test, it's expandable!
			}
		}

		public bool HasLength {
			get {
				return hasLength;
			}
		}

		public bool CustomLength {
			get {
				return customLength;
			}
		}

		public override void SetLength (long value)
		{
			if (hasLength || customLength) {			// If length was specified in constructor, let it be changed directly
				length = value;

				if (length < First.Length) {
					First.SetLength (length);
					Second.SetLength (0);
				} else {
					Second.SetLength (length - First.Length);
				}

				return;
			}

			// If the second stream doesn't support length or a custom length wasn't provided, we can't set length
			throw new ArgumentException ("The instance of ConcatStream does not support SetLength.");
		}

		public override long Position {
			get {
				if (!CanSeek)
					throw new NotSupportedException ("This instance of ConcatStream does not support Seeking.");
				return position;
			}
			set {
				if (!CanSeek)
					throw new NotSupportedException ("This instance of ConcatStream does not support Seeking.");

				if (value < 0) { value = 0; }							// Protect our bounds of valid position
				if (value > Length) { value = Length; }

				position = value;
			}
		}

		public override void Flush ()
		{
			First.Flush ();
			Second.Flush ();
			return;
		}

		public override long Seek (long offset, SeekOrigin origin)
		{
			if (!CanSeek)
				throw new NotSupportedException ("The concat stream in this instance does not support seeking.");

			switch (origin) {
				case SeekOrigin.Begin:
					Position = offset;
					break;
				case SeekOrigin.Current:
					Position = Position + offset;
					break;
				case SeekOrigin.End:
					if (hasLength || customLength) {			// If length was specified in constructor, we can do this
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
				throw new NotSupportedException ("The concat stream in this instance does not support reading.");

			int i = 0;

			while (i < count) {
				if (position >= First.Length) {						// If we need to write to second stream
					if (Second.CanSeek) {				
						Second.Position = Position - First.Length;	// Set position of second stream if we can
					}

					int bytesRead = 0;

					if (customLength) {								// If we have a custom length, we need to change count if it goes beyond our length
						int bytesLeft = (int)(length - position);
						if ((count - i) > bytesLeft)
							count = bytesLeft + i;
					}

					bytesRead = Second.Read (buffer, i + offset, count - i);	// Read what we can from second stream
					position = position + bytesRead;

					return bytesRead + i;
				}

				// Otherwise,
				First.Position = position;										// Set position of first stream

				int newCount = (int) Math.Min (First.Length - position, count);	// Calculate how many bytes we can read

				if (customLength) {												// If we have a custom length, we need to change count if it goes beyond our length
					int bytesLeft = (int)(length - position);
					if (newCount > bytesLeft)
						newCount = bytesLeft;
				}

				i = First.Read (buffer, offset, newCount);						// Read them
				position = position + i;										// Update position
			}

			return i;
		}

		public override void Write (byte[] buffer, int offset, int count)
		{
			// 		0	1	2	3	4	5	6	7	8	9
			//						P = 4							Length = 10;
			// 		Room Left = Length - Position = 10 - 4 =  6

			if (!CanWrite)
				throw new NotSupportedException ("The concat stream in this instance does not support writing.");

			int i = 0;

			while (i < count) {
				if (position >= First.Length) {
					if (Second.CanSeek) {
						Second.Position = Position - First.Length;
					}

					if (customLength) {								// If we have a custom length, we need to change count if it goes beyond our length
						int bytesLeft = (int)(length - position);
						if ((count - i) > bytesLeft)
							count = bytesLeft + i;
					}

					position = position + count - i;
					Second.Write (buffer, offset + i, count - i);
					return;
				}

				// Otherwise
				First.Position = position;
				i = (int)Math.Min (First.Length - position, count);

				if (customLength) {									// If we have a custom length, we need to change count if it goes beyond our length
					int bytesLeft = (int)(length - position);
					if (i > bytesLeft)
						i = bytesLeft;
				}

				First.Write (buffer, offset, i);
				position = position + i;
			}
		}
	}
}

