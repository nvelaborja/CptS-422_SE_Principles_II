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
	/// Wraps the given TextWriter. Adds line numbers to the beginning of each Write() output.
	/// </summary>
	public class NumberedTextWriter : System.IO.TextWriter
	{
		private TextWriter writer;
		private int lineNumber;

		/// <summary>
		/// Constructs a new NumberedTextWriter that wraps the given TextWriter. Line number starts at default (1).
		/// </summary>
		/// <param name="Writer">Writer.</param>
		public NumberedTextWriter (TextWriter Writer)
		{
			if (Writer == null)											// Throw an exception if a null Writer is given
				throw new NullReferenceException ();
			
			writer = Writer;
			lineNumber = 1;
		}

		/// <summary>
		/// Constructs a new NumberedTextWriter that wraps the given TextWriter. Line number starts at specified value.
		/// </summary>
		/// <param name="Writer">Writer.</param>
		/// <param name="lineNumber">Line number.</param>
		public NumberedTextWriter(TextWriter Writer, int LineNumber)
		{
			if (Writer == null)											// Throw an exception if a null Writer is given
				throw new NullReferenceException ();
			
			writer = Writer;
			lineNumber = LineNumber;
		}

		public override System.Text.Encoding Encoding 
		{
			get 
			{
				return writer.Encoding;	// Pass on the encoding of the wrapped TextWriter.
			}
		}

		public override void WriteLine (string value)
		{
			string numberedValue = lineNumber.ToString() + ": " + value; 	// Add line number to the beginning of the string value, then increment lineNumber
			writer.WriteLine(numberedValue);

			if (lineNumber == int.MaxValue)									// In case we go over the max value, start over at 1
				lineNumber = 0;
			lineNumber++;
		}

	}
}

