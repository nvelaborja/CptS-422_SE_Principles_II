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
		private Stream body;
		private NetworkStream response;
		private string httpMethod;
		private string httpVersion;
		private string uri;

		public WebRequest (Stream Body, NetworkStream Response)
		{
			body = Body;
			response = Response;
		}

		#region Properties

		public Dictionary<string,string> Headers
		{
			get{
				return headers;
			}
		}

		public Stream BodyStream
		{
			get{
				return body;
			}
		}

		public string HTTPMethod
		{
			get{
				return httpMethod;
			}
			set{
				httpMethod = value;
			}
		}

		public string HTTPVersion
		{
			get{
				return httpVersion;
			}
			set{
				httpVersion = value;
			}
		}

		public string URI
		{
			get{
				return uri;
			}
			set{
				uri = value;
			}
		}

		#endregion

		public long GetContentLengthOrDefault(long defaultValue)
		{
			// ??
		}
		public Tuple<long,long> GetRangeHeader()
		{
			// ??
		}

		public void WriteNotFoundResponse(string pageHTML)
		{


		}

		public bool WriteHTMLResponse(string htmlString)
		{

		}
	}
}

