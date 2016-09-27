using System;
using CS422;

namespace HW3
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			const string DefaultTemplate = 
				"HTTP/1.1 200 OK\r\n" + 
				"Content-Type: text/html\r\n" + 
				"\r\n\r\n" + 
				"<html>ID Number: {0}<br>" +
				"DateTime.Now: {1}<br>" +
				"Requested URL: {2}</html>";

			WebServer.Start (4000, DefaultTemplate);
		}
	}
}
