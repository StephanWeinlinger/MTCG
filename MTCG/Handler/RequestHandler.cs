using System;
using System.Net.Sockets;
using MTCG.Http;

namespace MTCG.Handler {
	static class RequestHandler {
		public static void Handle(TcpClient client) {
			try {
				// read request data, return body, url, method
				HttpRequest request = new HttpRequest(client);
				request.Log();

				// switch over different routes and call right controller method
				// returns answer string


			} catch(ArgumentException e) {
				// set new answer (error from exception)
				Console.WriteLine(e.Message);
			} catch(Exception e) {
				Console.WriteLine(e.Message);
			}
			finally {
				//HttpResponse response = new HttpResponse(client);
				//// send answer to client
				//response.Send();
			}
		}
	}
}
