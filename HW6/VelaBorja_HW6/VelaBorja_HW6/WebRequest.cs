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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace CS422
{
	public class WebRequest
	{
		private Dictionary<string,string> headers;
		private Stream bodyStream;
		private NetworkStream netStream;
		private string HTTPMethod;
		private string HTTPVersion;
		private string HTTPUri;
		private int length = -1;

		public WebRequest (NetworkStream netStream, byte[] buffer)								// buffer includes data we already got from the netStream in the Web Server
		{
			headers = new Dictionary<string, string> ();
			this.netStream = netStream;

			string previousData = System.Text.Encoding.ASCII.GetString (buffer).ToLower();		// Convert buffer to a string
			previousData = previousData.Substring(0, previousData.IndexOf("\r\n\r\n") + 1);			// Remove any body data

			// Parse header info into headers dictionary
			List<string> headerStrings = new List<string>(previousData.Split("\r\n".ToCharArray()));					// Get a list of whole header strings

			// First line is request info, parse it and remove from list
			GetRequestInfo(headerStrings[0]);
			headerStrings.RemoveAt (0);

			foreach (string header in headerStrings) {
				if (header == "")
					continue;
					string[] headerPieces = header.Split(":".ToCharArray(), 2, StringSplitOptions.None);								// header string should only have two pieces, one colon
				headers.Add(headerPieces[0].Trim(), headerPieces[1].Trim());			// Add dictionary entry for header
			}

			// Get the length
			length = GetLength();
			
			// Now we need to create the body stream
			CreateBodyStream(buffer);

		}

		public string Method
		{
			get{
				return HTTPMethod;
			}
		}

		public string Version
		{
			get{
				return HTTPVersion;
			}
		}

		public string Uri
		{
			get{
				return HTTPUri;
			}
		}

		public Stream BodyStream
		{
			get {
				return bodyStream;
			}
		}

		/// <summary>
		/// Creates a body stream out of possible body data already stuck in the buffer, and the netStream
		/// </summary>
		/// <returns><c>true</c>, if body stream was created, <c>false</c> otherwise.</returns>
		/// <param name="buffer">Buffer.</param>
		private bool CreateBodyStream(byte[] buffer)
		{
			// First see if there is any body data in buffer
			string bufferString = System.Text.Encoding.ASCII.GetString(buffer);
			bufferString.TrimEnd(new char[] { '\0' });
			List<string> bufferPieces = new List<string>(bufferString.Split (new string[] { "\r\n\r\n"}, StringSplitOptions.None));

			// Make sure second buffer piece isn't just a bunch of null characters
			if (bufferPieces.Count > 1) {
				for (int j = 0; j < bufferPieces [1].Length; j++) {
					if (bufferPieces [1] [j] == '\0') {
						bufferPieces [1] = bufferPieces [1].Remove (j, 1);
						j--;
					}
				}

				// Only keep the piece if there's any actual data in there
				if (bufferPieces [1].Length == 0)
					bufferPieces.RemoveAt (1);
			}

			// If not, the body stream is just the netStream
			if (bufferPieces.Count == 1) {
				bodyStream = netStream;
				//if (length > -1)
					//bodyStream.SetLength (length);
				return true;
			}

			// Otherwise, we need a new buffer that starts after the \r\n\r\n of the original buffer
			byte[] newBuffer = System.Text.Encoding.ASCII.GetBytes(bufferPieces[1]);

			// Create MemoryStream out of new buffer
			MemoryStream bufferStream = new MemoryStream(newBuffer);

			// Create ConcatStream with new memory stream and netStream
			if (length > -1)
				bodyStream = new ConcatStream(bufferStream, netStream, length);
			else
				bodyStream = new ConcatStream(bufferStream, netStream);

			return true;
		}

		private int GetLength() 			// Check to see if there is a Content-Length header
		{
			if (headers.ContainsKey ("Content-Length".ToLower ())) 
				return Convert.ToInt32(headers ["Content-Length".ToLower ()]);

			return -1;						// Return -1 to indicate no length
		}

		public int GetBodyLength()
		{
			// If there's header info on it, return that
			if (headers.ContainsKey ("Content-Length".ToLower ())) 					
				return Convert.ToInt32(headers ["Content-Length".ToLower ()]);

			// Otherwise we simply don't know for sure, so return 0
			return 0;
		}

		private bool GetRequestInfo(string info)
		{
			List<string> pieces = new List<string>(info.Split (" ".ToCharArray()));
			HTTPMethod = pieces [0];
			HTTPUri = pieces [1];
			HTTPVersion = pieces [2];

			return true;
		}

		public void WriteNotFoundResponse(string pageHTML)
		{
			string response = string.Format (
				"HTTP/1.1 404 Not Found\r\n" +
				"Content-Type: text/html\r\n" +
				"Content-Length: {0}\r\n" +
				"\r\n\r\n" + "{1}", System.Text.Encoding.ASCII.GetBytes(pageHTML).Length, pageHTML);

			byte[] responseInBytes = System.Text.Encoding.ASCII.GetBytes (response);

			netStream.Write (responseInBytes, 0, responseInBytes.Length); 

			return;
		}

		public bool WriteHTMLResponse(string htmlString)
		{
			string response = string.Format (
				"HTTP/1.1 200 OK\r\n" +
				"Content-Type: text/html\r\n" +
				"Content-Length: {0}\r\n" +
				"\r\n\r\n" + "{1}", System.Text.Encoding.ASCII.GetBytes(htmlString).Length, htmlString);

			byte[] responseInBytes = System.Text.Encoding.ASCII.GetBytes (response);

			netStream.Write (responseInBytes, 0, responseInBytes.Length); 

			return true;
		}
	}
}

