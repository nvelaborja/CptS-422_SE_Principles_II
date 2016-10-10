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
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CS422
{
	public class WebServer
	{
		//private static List<WebService> webServices;
		private static ConcurrentBag<WebService> webServices;
		private static BlockingCollection<TcpClient> tcpPool;
		private static Thread listenerThread;
		private static TcpListener listener;
		private static List<Thread> threads;
		private static int Port;

		public static bool Start(int port, int threadCount)
		{
			// Accept new TCP socket connection
			// Get a thread from the thread pool and pass it the TCP socket
			// Repeat

			webServices = new ConcurrentBag<WebService> ();
			threads = new List<Thread> ();
			tcpPool = new BlockingCollection<TcpClient> ();

			if (threadCount <= 0)
				threadCount = 64;

			Port = port;

			listenerThread = new Thread (() => {
				listener = new TcpListener (IPAddress.Any, port);
				listener.Start ();

				while (true) {
					TcpClient client = listener.AcceptTcpClient ();
					tcpPool.Add (client);
				}
			});

			listenerThread.Start ();

			for (int i = 0; i < threadCount; i++) {
				Thread newThread = new Thread (ThreadWork);
				threads.Add (newThread);
				newThread.Start ();
			}

			return true;
		}

		private static WebRequest BuildRequest(TcpClient client)
		{
			NetworkStream netStream = client.GetStream ();

			// ------ Start Reading from the Stream ------ \\

			byte[] buffer = new byte[4096];								// Storage for reading data, assuming requests won't be greater than 4KB
			string data = null;
			int bytesRead = 0;
			bool foundEnd = false;

			while (!foundEnd) { 
				int i = netStream.Read (buffer, bytesRead, buffer.Length - bytesRead);	// Receive request data from client
				bytesRead += i;
				data = System.Text.Encoding.ASCII.GetString (buffer, 0, bytesRead);		// Convert what we got to a string

				if (data.Contains ("\r\n\r\n") || bytesRead == buffer.Length)			// Keep reading until we find the \r\n\r\n or our buffer is full
					foundEnd = true;
			}

			// ------ Validate the request ------ \\

			if (!ValidateRequest (data)) {
				netStream.Dispose ();
				return null;
			}

			// If the request passes all our tests, build a request object
			WebRequest request = new WebRequest(netStream, buffer);

			return request;
		}

		/// <summary>
		/// This function will make sure the request fits all requirements for this server
		/// </summary>
		/// <param name="request">Request.</param>
		private static bool ValidateRequest(string data)
		{
			// Check to see if the request meets our restrictions
			if (data == null || data.Length == 0) { 					// Reject if no data was received
				return false;
			}

			if (!data.Contains ("\r\n\r\n")) {							// Reject if there was no double CRLF
				return false;
			}

			if (!data.StartsWith ("GET ")) {							// Reject requests other than GET
				return false;
			}

			if (!data.Contains (" HTTP/1.1")) {							// Reject HTTP versions other than 1.1
				return false;
			}

			// If HTTP version starts at any index before 6, there can't be any URL between GET and HTTP version
			if (data.IndexOf ("HTTP/1.1") < 6) {						// Reject if there is no URL between GET and HTTP
				return false;
			}

			return true;												// If pass all tests, return true
		}

		private static void ThreadWork()
		{
			TcpClient client = tcpPool.Take ();						// Will block until a client is put in the blocking collection

			if (client == null)
				return;

			WebRequest request = BuildRequest (client);
			bool serviced = false;

			if (request == null) {									// If we couldn't build a valid request
				client.GetStream ().Dispose ();
				client.Close();
				return;
			}

			foreach (WebService service in webServices) {
				if (service.ServiceURI == request.Uri) {
					serviced = true;
					service.Handler (request);
					break;
				}
			}

			if (!serviced)
				request.WriteNotFoundResponse (request.Uri);

			client.GetStream ().Dispose ();
			client.Close ();

		}

		public static void AddService(WebService service)
		{
			webServices.Add (service);
		}

		/// <summary>
		/// This is a blocking function that lets all threads in the thread pool finish the current task they're on, then terminates all threads.
		/// </summary>
		public static void Stop()
		{
			tcpPool.CompleteAdding ();

			listener.Stop ();

			for (int i = 0; i < threads.Count; i++) {
				tcpPool.Add (null);
			}

			foreach (Thread thread in threads) {
				thread.Join ();
			}
		}
	}
}

