﻿/****************************************************\
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
	internal class DemoService : WebService
	{
		private const string c_template = 
			"<html>This is the response to the request:<br>" + 
			"Method: {0}<br>Request-Target/URI: {1}<br>" + 
			"Request body size, in bytes: {2}<br>" +
			"Date: {3}<br><br>" +
			"Student ID: {4}<br>" + 
			"Thanks for grading my assignment <3<br>" +
			"</html>";

		public DemoService ()
		{
			
		}

		public override void Handler(WebRequest req)
		{
			// Write response
			req.WriteHTMLResponse(string.Format(c_template, req.Method.ToUpper(), req.Uri, req.GetBodyLength(), DateTime.Now, 11392441));
		}

		public override string ServiceURI 
		{
			get {
				return "/";
			}
		}
	}
}

