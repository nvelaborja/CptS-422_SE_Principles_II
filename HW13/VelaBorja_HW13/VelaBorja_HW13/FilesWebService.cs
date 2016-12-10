/***********************************\
 * Nathan VelaBorja
 * October 24, 2016
 * Cpts 422 HW 9
 * Collaborators: 
 * 	None
\***********************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CS422
{
	internal class FilesWebService : WebService
	{
		private FileSys422 m_fileSys;
		private bool m_allowUploads;
		#region Don't Look In Here
		private const string error404 = 
			"<html>" + 
				"………………….._,,-~’’’¯¯¯’’~-,,<br>"+
				"………………..,-‘’ ; ; ;_,,---,,_ ; ;’’-,…………………………….._,,,---,,_<br>"+
				"……………….,’ ; ; ;,-‘ , , , , , ‘-, ; ;’-,,,,---~~’’’’’’~--,,,_…..,,-~’’ ; ; ; ;__;’-,<br>"+
				"……………….| ; ; ;,’ , , , _,,-~’’ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ¯’’~’-,,_ ,,-~’’ , , ‘, ;’,<br>"+
				"……………….’, ; ; ‘-, ,-~’’ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;’’-, , , , , ,’ ; |<br>"+
				"…………………’, ; ;,’’ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;’-, , ,-‘ ;,-‘..............................<br>"+
				"………………….,’-‘ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;’’-‘ ;,,-‘................................................<br>"+
				"………………..,’ ; ; ; ; ; ; ; ; ; ; ; ;__ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ‘-,’..........<b>404 NOT FOUND!</b>......................<br>"+
				"………………,-‘ ; ; ; ; ; ; ; ; ; ;,-‘’¯: : ’’-, ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; _ ; ; ; ; ;’,........Sorry mate, but what....................<br>"+
				"……………..,’ ; ; ; ; ; ; ; ; ; ; ;| : : : : : ; ; ; ; ; ; ; ; ; ; ; ; ,-‘’¯: ¯’’-, ; ; ;’,..../....you're lookin for ain't................<br>"+
				"…………….,’ ; ; ; ; ; ; ; ; ; ; ; ‘-,_: : _,-‘ ; ; ; ; ; ; ; ; ; ; ; ; | : : : : : ; ; ; |../........here!..............................<br>"+
				"……………,’ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ¯¯ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;’-,,_ : :,-‘ ; ; ; ;|......................................<br>"+
				"…………..,-‘ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ,,-~’’ , , , , ,,,-~~-, , , , _ ; ; ;¯¯ ; ; ; ; ;|....................................<br>"+
				"..…………,-‘ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;,’ , , , , , , ,( : : : : , , , ,’’-, ; ; ; ; ; ; ; ;|<br>"+
				"……….,-‘ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;’, , , , , , , , ,’~---~’’ , , , , , ,’ ; ; ; ; ; ; ; ;’,<br>"+
				"…….,-‘’ ; _, ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ‘’~-,,,,--~~’’’¯’’’~-,,_ , ,_,-‘ ; ; ; ; ; ; ; ; ; ‘,<br>"+
				"….,-‘’-~’’,-‘ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; | ; ; | . . . . . . ,’; ,’’¯ ; ; ; ; ; ; ; ; ; ,_ ; ‘-,<br>"+
				"……….,’ ; ;,-, ; ;, ; ; ;, ; ; ; ; ; ; ; ; ; ; ‘, ; ;’, . . . . .,’ ;,’ ; ; ; ;, ; ; ;,’-, ; ;,’ ‘’~--‘’’<br>"+
				"………,’-~’ ,-‘-~’’ ‘, ,-‘ ‘, ,,- ; ; ; ; ; ; ; ; ‘, ; ; ‘~-,,,-‘’ ; ,’ ; ; ; ; ‘, ;,-‘’ ; ‘, ,-‘,<br>"+
				"……….,-‘’ ; ; ; ; ; ‘’ ; ; ;’’ ; ; ; ; ; ; ; ; ; ; ‘’-,,_ ; ; ; _,-‘ ; ; ; ; ; ;’-‘’ ; ; ; ‘’ ; ;’-,<br>"+
				"……..,-‘ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;¯¯’’¯ ; ; ; ; ; ; ; ; , ; ; ; ; ; ; ; ; ;’’-,<br>"+
				"……,-‘ ; ; ; ; ; ; ; ,, ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; |, ; ; ; ; ; ; ; ; ; ; ‘-,<br>"+
				"…..,’ ; ; ; ; ; ; ; ;,’ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;|..’-,_ ; ; ; , ; ; ; ; ; <br>"+
				"….,’ ; ; ; ; ; ; ; ; | ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;,’…….’’’,-~’ ; ; ; ; ; ,’<br>"+
				"…,’ ; ; ; ; ; ; ; ; ;’~-,,,,,--~~’’’’’’~-,, ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;,’…..,-~’’ ; ; ; ; ; ; ,-<br>"+
				"…| ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ‘, ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;,’…,-‘ ; ; ; ; ; ; ; ;,-‘<br>"+
				"…’, ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ,-‘ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ,’….’, ; ; ; ; _,,-‘’<br>"+
				"….’, ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ,-‘’ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;,’…….’’~~’’¯<br>"+
				"…..’’-, ; ; ; ; ; ; ; ; ; ; ; ; ; ;_,,-‘’ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ,-‘<br>"+
				"………’’~-,,_ ; ; ; ; _,,,-~’’ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;,-‘<br>"+
				"………..| ; ; ;¯¯’’’’¯ ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;,,-‘<br>"+
				"………..’, ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;,-‘<br>"+
				"…………| ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ; ;|<br>"+
				"…………’, ; ; ; ; ; ; ; ; ; ~-,,___ ; ; ; ; ; ; ; ; ; ; ; ; ; ;’,<br>"+
				"………….’, ; ; ; ; ; ; ; ; ; ; ;,-‘….’’-, ; ; ; ; ; ; ; ; ; ; ; ; ‘,<br>"+
				"………..,’ ‘- ; ; ; ; ; ; ; ; ;,-‘’……….’-, ; ; ; ; ; ; ; ; ; ; ; ‘,<br>"+
				"……….,’ ; ;’ ; ; ; ; ; ; ,,-‘…………….’, ; ; ; ; ; ; ; ; ; ; ;’,<br>"+
				"………,’ ; ; ; ; ; ; ; ;,-‘’…………………’’-, ; ; ; ; ; ; ; ; ; |<br>"+
				"……..,’ ; ; ; ; ; ; ;,,-‘………………………’’, ; ; ; ; ; ; ; ; |<br>"+
				"……..| ; ; ; ; ; ; ;,’…………………………,’ ; ; ; ; ; ; ; ;,’<br>"+
				"……..| ; ; ; ; ; ; ,’………………………..,-‘ ; ; ; ; ; ; ; ,’’<br>"+
				"……..| ; ; ; ; ; ;,’……………………….,-‘ ; ; ; ; ; ; ; ,-‘<br>"+
				"……..’,_ , ; , ;,’……………………….,’ ; ; ; ; ; ; ; ,-‘<br>"+
				"………’,,’,¯,’,’’|……………………….| ; ; ; ; ; ; ; ; ‘--,,<br>"+
				"………….¯…’’………………………..’-, ; ; ; ; ; ; ; ; ; ;’’~,,<br>"+
				"……………………………………………’’-,, ; ; ; ; ; ; ; ; ; ;’’~-,,<br>"+
				"………………………………………………..’’-, ; ; ; ; ; ,,_ ; ;’-,’’-,<br>"+
				"…………………………………………………..’, ; ; ; ; ; ; ‘-,__,--.<br>"+
				"……………………………………………………’-, ; ; ;,,-~’’’ , ,|, |<br>"+
				"………………………………………………………’’~-‘’_ , , ,,’,_/--‘<br>"+
			"</html>";
		#endregion
		private const string ok200 = "<html>Thanks for uploading!<br></html>";
		private const string ok200bad = "<html>Your upload failed!<br></html>";

		public FilesWebService (FileSys422 fileSystem)
		{
			m_fileSys = fileSystem;
			m_allowUploads = true;
		}

		public override string ServiceURI 
		{
			get {
				return "/files";
			}
		}

		public override void Handler(WebRequest req)
		{
			if (req.Method == "GET") {
				GETHandler (req);
				return;
			}

			if (req.Method == "PUT") {
				PUTHandler (req);
				return;
			}
		}

		private void GETHandler(WebRequest req)
		{
			// First remove the /files part of the request uri
			string URI = req.Uri.Substring (6, req.Uri.Length - 6);					// If we reach this Handler, the request URI MUST start with /files, so we can hardcode it's removal
			string parentPath = "";
			Dir422 parent = m_fileSys.GetRoot ();

			// We need to find out if the requested file/dir is valid in our fileSys
			// Start by cutting up uri into pieces. All piece but the last should be a dir. The last could be dir or file or rubbish
			string[] uriPieces = URI.Split (new char[]{'/'}, StringSplitOptions.RemoveEmptyEntries);

			// If there are no pieces, they must have requested root
			if (uriPieces.Length == 0) {
				req.WriteHTMLResponse (BuildDirHTML (parent, ""));
				return;
			}

			for (int i = 0; i < uriPieces.Length - 1; i++) {						// Use our parent node to navigate to parent dir
				if (parent == null) {												// If we ever fail in this navigation, the path must be wrong
					Error404 (req);													// Write 404 and return
					return;
				}
				parent = parent.GetDir (uriPieces [i]);

				if (parent != null)
					parentPath += ("/" + parent.Name);
			}

			string name = uriPieces [uriPieces.Length - 1];							// Get the name of the requested dir/file
			parentPath += ("/" + name);												// Don't forget to add it to our parentPath so we know where we came from

			if (parent.ContainsFile(name, false)) {										// If the requested object exists is a valid file
				SendFile (req, parent.GetFile (name));
				return;
			}

			if (parent.ContainsDir (name, false)) {										// If the requested object exists is a valid dir
				// Make sure the uri ends with '/' in case it didn't already
				req.WriteHTMLResponse (BuildDirHTML (parent.GetDir (name), parentPath));		// Write back directory display
				return;
			}

			// If the requested object does not exist, we write a 404
			Error404 (req);
		}

		private void PUTHandler(WebRequest req)
		{
			// First remove the /files part of the request uri
			string URI = req.Uri.Substring (6, req.Uri.Length - 6);					// If we reach this Handler, the request URI MUST start with /files, so we can hardcode it's removal
			Dir422 parent = m_fileSys.GetRoot ();

			// We need to find out if the requested file/dir is valid in our fileSys
			// Start by cutting up uri into pieces. All piece but the last should be a dir. The last could be dir or file or rubbish
			string[] uriPieces = URI.Split (new char[]{'/'}, StringSplitOptions.RemoveEmptyEntries);

			// If there are no pieces, how can they upload anything?
			if (uriPieces.Length == 0) {
				req.WriteHTMLResponse (ok200bad);
				return;
			}

			// We need to navigate to the parent dir specified in the uri
			for (int i = 0; i < uriPieces.Length - 1; i++) {						// Use our parent node to navigate to parent dir
				if (parent == null) {												// If we ever fail in this navigation, the path must be wrong
					Error404 (req);													// Write 404 and return
					return;
				}
				parent = parent.GetDir (uriPieces [i]);
			}

			// Now we know our parent exists, we can get the data and write it to a file
			string fileName = uriPieces [uriPieces.Length - 1];

			if (parent.ContainsFile (fileName, false) || fileName.Contains ("\\")) {	// If the parent dir already has a file by that name or illegal chars, notify of upload failure and return
				req.WriteHTMLResponse (ok200bad);
				return;
			}

			File422 newFile = parent.CreateFile (fileName);

			FileStream fs = (FileStream)newFile.OpenReadWrite ();
			bool endOfFile = false;
			long totalBytesRead = 0;

			while (!endOfFile) {
				byte[] buffer = new byte[4096];
				int bytesRead = 0;

				try {
					if (req.BodyStream.CanRead)
						bytesRead = req.BodyStream.Read (buffer, 0, buffer.Length);
					if (req.BodyStream.CanWrite)
						fs.Write (buffer, 0, bytesRead);
				} catch (IOException ex) {
					endOfFile = true;
				}

				if (bytesRead < buffer.Length)
					endOfFile = true;

				totalBytesRead += bytesRead;
			}

			req.WriteHTMLResponse (ok200);
		}

		private void SendFile(WebRequest req, File422 file)
		{
			Stream fileStream = file.OpenReadOnly ();
			NetworkStream netStream = req.NetStream;

			bool sizeCap = req.ContainsKey ("content-range");

			if (!sizeCap) {														// If there's no range header, simply stream the file data
				// First write the header response
				string responseHeader = string.Format (
					"HTTP/1.1 200 OK\r\n" +
					"Content-Type: {0}\r\n" +
					"Content-Length: {1}" +
					"\r\n\r\n", GetContentType(file), fileStream.Length);

				byte[] headerData = System.Text.Encoding.ASCII.GetBytes (responseHeader);

				netStream.Write(headerData, 0, headerData.Length);

				// Then write the file data
				byte[] fileData = new byte[4096];									// Read/write file data 4KB at a time

				while (true) {
					int bytesRead = fileStream.Read (fileData, 0, fileData.Length);
					if (bytesRead == 0) { break; }

					try {
						netStream.Write(fileData, 0, bytesRead);					// This write sometimes crashes due to browser closing the socket, not sure what to do
					} catch (Exception ex) {
						return;
					}
				}

				fileStream.Close ();
				return;
			}

			#region Partial Content Request

			string partialResponseHeader = string.Format (
				"HTTP/1.1 206 Partial Content\r\n" +
				"Content-Type: {0}\r\n" +
				"Content-Length: {1}" +
				"\r\n\r\n", GetContentType(file), fileStream.Length);

			string partialRequest = req.GetHeaderValue ("content-range");
			int lowerBound = 0;															// Represents where they want to start reading from in bytes
			int count = 0;																// Represents how many bytes they want starting at lowerBound
			GetBounds (partialRequest, ref lowerBound, ref count); 

			byte[] partialHeaderData = System.Text.Encoding.ASCII.GetBytes (partialResponseHeader);

			netStream.Write(partialHeaderData, 0, partialHeaderData.Length);

			// Then write the file data
			byte[] partialFileData = new byte[4096];									// Read/write file data 4KB at a time
			fileStream.Seek (lowerBound, SeekOrigin.Begin);								// Seek to where the partial request starts

			int partialBytesRead = fileStream.Read (partialFileData, 0, 4096);
			int totalBytesRead = 0;

			while (partialBytesRead > 0 && totalBytesRead < count) {
				netStream.Write(partialFileData, 0, partialBytesRead);
				partialBytesRead = fileStream.Read (partialFileData, 0, Minimum(4096, count - totalBytesRead));
			}

			fileStream.Close();

			#endregion
		}

		private string BuildDirHTML(Dir422 directory, string parentPath)
		{
			var htmlBuilder = new StringBuilder ("<html>");

			if (m_allowUploads) {
				htmlBuilder.AppendLine (
					@"<script>
						function selectedFileChanged(fileInput, urlPrefix){
							document.getElementById('uploadHdr').innerText = 'Uploading ' + fileInput.files[0].name + '...';
							// Need XMLHttpRequest to do the upload
							if (!window.XMLHttpRequest) {
								alert('Your browser does not support XMLHttpRequest. Please update your browser.');
								return;
							}
							// Hide the file selection controls while we upload
							var uploadControl = document.getElementById('uploader');
							if (uploadControl){
								uploadControl.style.visibility = 'hidden';
							}
							// Build a URL for the request
							if (urlPrefix.lastIndexOf('/') != urlPrefix.length - 1){
								urlPrefix += '/';
							}
							var uploadURL = urlPrefix + fileInput.files[0].name;
							// Create the service request object
							var req = new XMLHttpRequest();
							req.open('PUT', uploadURL);
							req.onreadystatechange = function(){
								document.getElementById('uploadHdr').innerText = 'Upload (request status == ' + req.status + ')';
							};
							req.send(fileInput.files[0]);
						}
					</script>");
			}

			htmlBuilder.AppendLine("<h1>FOLDERS</h1>");
			htmlBuilder.AppendLine("<ul>");
			htmlBuilder.AppendLine( GetFolderHREFS(directory, parentPath) + "<br>");
			htmlBuilder.AppendLine("</ul>");
			htmlBuilder.AppendLine("<h1>FILES</h1>");
			htmlBuilder.AppendLine("<ul>");
			htmlBuilder.AppendLine( GetFileHREFS(directory, parentPath) + "<br><br>");
			htmlBuilder.AppendLine("</ul>");

			if (m_allowUploads) {
				htmlBuilder.AppendLine("<hr><h3 id='uploadHdr'>Upload</h3><br>");
				htmlBuilder.AppendLine("<input id=\"uploader\" type='file' ");
				htmlBuilder.AppendFormat("onchange='selectedFileChanged(this,\"/files{0}/\")' /><hr>", parentPath);
			}

			htmlBuilder.AppendLine("<hr>");
			htmlBuilder.AppendLine("Nathan VelaBorja's File Explorer");
			htmlBuilder.AppendLine("</html>");

			return htmlBuilder.ToString();
		}

		private void Error404(WebRequest req)
		{
			req.WriteNotFoundResponse (error404);
		}

		private void OK200(WebRequest req)
		{
			req.WriteHTMLResponse (ok200);
		}

		private void GetBounds(string request, ref int lower, ref int count)
		{
			request = request.Replace ("bytes ", "");
			request = request.Remove (request.IndexOf ('/'), request.Length - request.IndexOf ('/'));
			string[] pieces = request.Split (new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

			if (pieces.Length < 2)
				return;

			lower = Convert.ToInt32 (pieces [0]);
			count = Convert.ToInt32 (pieces [1]) - lower;
		}

		private string GetContentType(File422 file)
		{
			string type = file.FileType.ToUpper();
			string contentType = "";

			switch (type) {
			case "JPEG":
				contentType = "image/jpeg";
				break;
			case "JPG":
				contentType = "image/jpeg";
				break;
			case "PNG":
				contentType = "image/png";
				break;
			case "PDF":
				contentType = "application/pdf";
				break;
			case "MP4":
				contentType = "video/mp4";
				break;
			case "TXT":
				contentType = "text/plain";
				break;
			case "HTML":
				contentType = "text/html";
				break;
			case "XML":
				contentType = "application/xml";
				break;
			default:
				contentType = "text/html";
				break;
			}

			return contentType;
		}

		private int Minimum(int value1, int value2) 
		{
			if (value1 <= value2)
				return value1;
			return value2;
		}

		private string GetFolderHREFS(Dir422 directory, string parentPath)
		{
			List<Dir422> folders = (List<Dir422>)directory.GetDirs ();

			string folderList = "";

			// Start folder list with a ".." entry if it's not root dir
			if (directory != m_fileSys.GetRoot()) {
				folderList += "<li><a href=\"/files" + parentPath + "/../\">..</a></li>";
			}

			foreach (Dir422 folder in folders) {
				folderList += string.Format ("<li><a href=\"/files{0}/{1}\">{1}</a></li>", parentPath, folder.Name);
			}

			return folderList;
		}

		private string GetFileHREFS(Dir422 directory, string parentPath)
		{
			List<File422> files = (List<File422>)directory.GetFiles ();

			string fileList = "";

			foreach (File422 file in files) {
				fileList += string.Format ("<li><a href=\"/files{0}/{1}\">{1}</a></li>", parentPath, file.Name);
			}

			return fileList;
		}
	}
}

