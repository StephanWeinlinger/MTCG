using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Http {
	class HttpResponse {
		public string Content { get; set; }
		public Dictionary<string, string> Headers { get; set; }
		// type of respone OK, ...

		private StreamWriter _writer;

		public HttpResponse(TcpClient client) {
			_writer = new (client.GetStream()) { AutoFlush = true };
			
		}

		public void Send() {
			// write header
			foreach(KeyValuePair<string, string> entry in Headers) {
				_writer.WriteLine($"{entry.Key}: {entry.Value}");
			}
			_writer.WriteLine("");
			// write content
		}

	}
}
