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
		public IDictionary<string, string> Headers { get; set; }

		private StreamWriter _writer;

		public HttpResponse(TcpClient client) {
			_writer = new (client.GetStream()) { AutoFlush = true };
			Headers = new Dictionary<string, string>();
			SetDefaultHeaders();
		}

		private void SetDefaultHeaders() {
			Headers.Add("Server", "MTCG-Weinlinger");
			Headers.Add("Content-Type", "application/json; charset=utf-8");
		}

		public void Send(ResponseContent.ResponseContent responseContent) {
			// write header
			_writer.WriteLine($"HTTP/1.1 {responseContent.Code} {responseContent.Message}");
			foreach(KeyValuePair<string, string> entry in Headers) {
				_writer.WriteLine($"{entry.Key}: {entry.Value}");
			}
			_writer.WriteLine("");
			// write content
			_writer.WriteLine(responseContent.Content);
			_writer.Flush();
			_writer.Close();
		}

	}
}
