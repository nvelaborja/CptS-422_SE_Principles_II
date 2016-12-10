/***********************************\
 * Nathan VelaBorja
 * October 24, 2016
 * Cpts 422 HW 9
 * Collaborators: 
 * 	None
\***********************************/

using System;
using CS422;

namespace VelaBorja_HW13
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Starting Web Server..");

			WebServer.Start (4000, 30);

			FilesWebService fileService = new FilesWebService (StandardFileSystem.Create ("/home/nathan/Documents"));

			WebServer.AddService (fileService);
		}
	}
}
