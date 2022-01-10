using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MTCG.Handler;

namespace MTCG.Http {

	public class HttpServer {
		private readonly int _port;

		public HttpServer(int port) {
			_port = port;
		}

		public void Run() {
			TcpListener listener = new TcpListener(IPAddress.Loopback, _port);
			listener.Start(5);
			while(true) {
				TcpClient client = listener.AcceptTcpClient();
				Console.WriteLine("Request came in!");
				Task.Run(() => {
					RequestHandler.Handle(client);
				});
			}
		}
	}
}
