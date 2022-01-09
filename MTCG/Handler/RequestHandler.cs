using System;
using System.Net.Sockets;
using MTCG.Controller;
using MTCG.Exception;
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
						responseContent = new UserController(request).ResponseContent;
						break;
					case "sessions":
						responseContent = new SessionController(request).ResponseContent;
						break;
					case "packages":
						responseContent = new PackageController(request).ResponseContent;
						break;
					case "transactions":
						responseContent = new TransactionController(request).ResponseContent;
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
					case "battles":
						responseContent = new BattleController(request).ResponseContent;
						break;
					default:
						responseContent = new ResponseNotFound();
						break;
				}
			} catch(FailedAuthException e) {
				responseContent = new ResponseUnauthorized(e.Message, true);
			} catch(BadRequestException e) {
				responseContent = new ResponseBadRequest(e.Message, true);
			} catch(System.Exception e) {
				Console.WriteLine(e.Message);
				responseContent = new ResponseBadRequest("Unknown error", true);
			}
			finally {
				HttpResponse response = new HttpResponse(client);
				// send answer to client
				response.Send(responseContent);
			}
		}
	}
}