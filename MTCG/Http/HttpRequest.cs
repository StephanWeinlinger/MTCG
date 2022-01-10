using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MTCG.Exception;

namespace MTCG.Http {
	class HttpRequest {
		public HttpMethod Method { get; private set; }
		public string FullPath { get; private set; }
		public string Content { get; private set; }
		public string Authorization { get; private set; }
		public IDictionary<string, string> Headers { get; }
		public IList<string> PathContents { get; set; }

		private StreamReader _reader;

		public HttpRequest(TcpClient client) {
			_reader = new(client.GetStream());
			Headers = new Dictionary<string, string>();
			PathContents = new List<string>();
			Method = HttpMethod.Undefined;
			Content = "";
			Authorization = "";
			Parse();
		}

		private void Parse() {
			GetHead();
			// read body on POST and PUT
			if((Method == HttpMethod.POST || Method == HttpMethod.PUT) && Headers.ContainsKey("Content-Length")) {
				GetContent();
			}
			ParseAuthorization();
			ParsePath();
		}

		private void GetHead() {
			string line = null;
			while((line = _reader.ReadLine()) != null) {
				if(line.Length == 0) {
					break;
				}

				if(Method == HttpMethod.Undefined) {
					var parts = line.Split(' ');
					if(Enum.TryParse(parts[0], out HttpMethod tmpMethod)) {
						Method = tmpMethod;
					} else {
						throw new BadRequestException("Method not supported");
					}
					FullPath = WebUtility.UrlDecode(parts[1]);
				}
				// handle headers
				else {
					var parts = line.Split(": ");
					Headers.Add(parts[0], parts[1]);
				}
			}
		}

		private void GetContent() {
			if(int.TryParse(Headers["Content-Length"], out int length)) {
				char[] buffer = new char[length];
				_reader.Read(buffer, 0, length);
				Content = new string(buffer);
			}
		}

		private void ParseAuthorization() {
			if(Headers.ContainsKey("Authorization")) {
				var parts = Headers["Authorization"].Split(" ");
				if(parts.Length == 2) {
					Authorization = parts[1];
				}
			}
		}

		private void ParsePath() {
			// get path contents
			string[] parts = FullPath.Split("?");
			PathContents = parts[0].Substring(1).Split("/").ToList();
		}
	}
}

