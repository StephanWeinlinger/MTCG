using System;
using System.Net.Sockets;
using MTCG.Controller;
using MTCG.Http;
using MTCG.Http.ResponseContent;

namespace MTCG.Handler {
	static class RequestHandler {
		public static void Handle(TcpClient client) {
			ResponseContent responseContent = new ResponseBadRequest();
			try {
				// read request data, return body, url, method
				HttpRequest request = new HttpRequest(client);
				//request.Log();
				// switch over different routes and call right controller method
				switch(request.PathContents[0]) {
					case "users":
						var controller = new UserController(request);
						controller.Handle();
						responseContent = controller.ResponseContent;
						break;
					case "sessions":
						break;
					case "packages":
						break;
					case "transactions":
						break;
					case "cards":
						break;
					case "deck":
						break;
					case "stats":
						break;
					case "score":
						break;
					case "tradings":
						break;
					default:
						responseContent = new ResponseNotFound();
						break;
				}
			} catch(Exception e) {
				responseContent = new ResponseBadRequest();
				Console.WriteLine(e.Message);
			}
			finally {
				HttpResponse response = new HttpResponse(client);
				// send answer to client
				response.Send(responseContent);
			}
		}
	}
}