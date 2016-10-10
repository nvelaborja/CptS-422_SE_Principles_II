/****************************************************\
 * 		Nathan VelaBorja
 * 		September 14, 2016
 * 		CptS 422 HW 3
 *		This assignment simulates a basic web server 
 * 		  that accepts GET requests and writes back
 * 		  whatever url was requested along with the
 * 		  current DateTime and my WSUID.
 \***************************************************/

using System;
using System.Net; 
using System.Net.Sockets;

namespace CS422
{
	public class WebServer
	{


		public WebServer ()
		{

		}

		public static bool Start(int port, string responseTemplate)
		{
			// ------ Create Listener and Client ------ \\

			TcpListener listener = new TcpListener (IPAddress.Any, port);
			listener.Start ();
			TcpClient client = null;

			try {														// Not sure if failure results in an exception, so do this in a try
				client = listener.AcceptTcpClient ();
			} catch {
				if (client == null)										// If listener failed to find a client, return false
					return false;
			}
				
			if (client == null)											// If listener failure doesn't create an exception, still exit
				return false;

			// ------ Start Reading from the Stream ------ \\

			byte[] buffer = new byte[4096];								// Storage for reading data, assuming requests won't be greater than 4KB
			string data = null;
			int bytesRead = 0;
			bool foundEnd = false;

			NetworkStream netStream = client.GetStream ();				// Get the stream from the client

			while (!foundEnd) { 
				int i = netStream.Read (buffer, bytesRead, buffer.Length - bytesRead);	// Receive request data from client
				bytesRead += i;
				data = System.Text.Encoding.ASCII.GetString (buffer, 0, bytesRead);		// Convert what we got to a string

				if (data.Contains ("\r\n\r\n") || bytesRead == buffer.Length)			// Keep reading until we find the \r\n\r\n or our buffer is full
					foundEnd = true;
			}

			// ------ Start processing the request ------ \\

			// Check to see if the request meets our restrictions
			if (data == null || data.Length == 0) { 					// Reject if no data was received
				netStream.Dispose ();
				return false;
			}

			if (!data.Contains ("\r\n\r\n")) {							// Reject if there was no double CRLF
				netStream.Dispose ();
				return false;
			}

			if (!data.StartsWith ("GET ")) {							// Reject requests other than GET
				netStream.Dispose ();
				return false;
			}

			if (!data.Contains (" HTTP/1.1")) {							// Reject HTTP versions other than 1.1
				netStream.Dispose ();
				return false;
			}

			// If HTTP version starts at any index before 6, there can't be any URL between GET and HTTP version
			if (data.IndexOf ("HTTP/1.1") < 6) {							// Reject if there is no URL between GET and HTTP
				netStream.Dispose ();
				return false;
			}

			// ------ Start Mining Request ------ \\

			// See what they want!
			string URL = data.Substring(0, data.IndexOf("HTTP/1.1"));	// URL will be after GET and before HTTP
			URL = URL.Substring(4);										// Cut off the "GET " from the beginning
			URL = URL.Trim();											// Cut off the space after the url

			// ------ Write Back to Client ------ \\

			// Write back!
			string response = string.Format(responseTemplate, "11392441", DateTime.Now.ToString(), URL);
			byte[] responseInBytes = System.Text.Encoding.ASCII.GetBytes (response);
			netStream.Write (responseInBytes, 0, responseInBytes.Length);

			netStream.Dispose ();
			return true;
		}
	}
}

