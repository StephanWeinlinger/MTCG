using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MTCG.Http {
	class HttpRequest {
		public HttpMethod Method { get; private set; }
		public string FullPath { get; private set; }
		public string Content { get; private set; }
		public Dictionary<string, string> Headers { get; }
		public List<string> PathContents { get; set; }
		public Dictionary<string, string> QueryParameters { get; set; }

		private StreamReader _reader;

		public HttpRequest(TcpClient client) {
			_reader = new(client.GetStream());
			Headers = new();
			Method = HttpMethod.Undefined;
			Parse();
		}

		private void Parse() {
			GetHead();
			// read body on POST and PUT
			if(Method == HttpMethod.POST || Method == HttpMethod.PUT) {
				GetContent();
			}
			ParsePath();
		}

		public void Log() {
			Console.WriteLine(Method);
			Console.WriteLine(FullPath);
			foreach(KeyValuePair<string, string> entry in Headers) {
				Console.WriteLine($"{entry.Key}: {entry.Value}");
			}
			foreach(string entry in PathContents) {
				Console.WriteLine($"{entry}");
			}
			foreach(KeyValuePair<string, string> entry in QueryParameters) {
				Console.WriteLine($"{entry.Key}: {entry.Value}");
			}
			Console.WriteLine(Content);
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
						throw new ArgumentException("Method not supported");
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
			int length = int.Parse(Headers["Content-Length"]);
			char[] buffer = new char[length];
			_reader.Read(buffer, 0, length);
			Content = new string(buffer);
		}

		private void ParsePath() {
			// get path contents
			string[] parts = FullPath.Split("?");
			PathContents = parts[0].Split("/").ToList();
			// get query parameters
			NameValueCollection tmpParameters = HttpUtility.ParseQueryString(parts[1]);
			// convert to dictionary
			QueryParameters = tmpParameters.AllKeys.ToDictionary(t => t, t => tmpParameters[t]);
		}
	}
}

