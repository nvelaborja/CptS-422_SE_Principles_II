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

namespace CS422
{
	public class DemoService : WebRequest
	{
		private const string c_template = 
			"<html>This is the response to the request:<br>" + 
			"Method: {0}<br>Request-Target/URI: {1}<br>" + 
			"Request body size, in bytes: {2}<br><br>" +
			"Student ID: {3}</html>";

		public DemoService ()
		{
		}

		public override string ServiceURI 
		{
			get {
				return "/";
			}
		}
	}
}

