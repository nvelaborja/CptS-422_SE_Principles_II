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

		public WebRequest ()
		{
		}

		public void WriteNotFoundResponse(string pageHTML)
		{

		}

		public bool WriteHTMLResponse(string htmlString)
		{

		}
	}
}

