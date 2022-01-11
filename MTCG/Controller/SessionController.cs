using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Crypto;
using MTCG.Database.Storage;
using MTCG.Database.Table;
using MTCG.Exception;
using MTCG.Http;
using MTCG.Http.ResponseContent;
using MTCG.Parse;

namespace MTCG.Controller {
	public sealed class SessionController : Controller {
		public SessionController(HttpRequest request) : base(request) {
			Handle();
		}

		public override void Handle() {
			string path = Request.PathContents[0];
			switch(Request.Method) {
				case HttpMethod.GET:
						CheckAuth();
						LogoutUser();
					break;
				case HttpMethod.POST:
						LoginUser();
					break;
				default:
					ResponseContent = new ResponseNotFound();
					break;
			}
		}

		private void LoginUser() {
			var data = JsonDeserializer.Deserialize<Dictionary<string, string>>(Request.Content, DeserializeType.LOGIN_USER);
			Database.Data = new Dictionary<string, string> {
				{ "username", data["username"] }
			};
			UserStorage user = UserTable.GetUserByUsername(Database);
			if(user == null) {
				throw new BadRequestException("User could not be found");
			}
			if(!Hash.CompareString(data["password"], user.Password)) {
				throw new BadRequestException("Wrong password");
			}
			Database.Data = new Dictionary<string, string> {
				{ "isloggedin", "true" },
				{ "id", user.Id.ToString() }
			};
			int id = UserTable.UpdateIsLoggedIn(Database);
			if(id == -1) {
				throw new BadRequestException("Login failed");
			}
			ResponseContent = new ResponseOk(JsonSerializer.Serialize(user.Token), false);
		}

		private void LogoutUser() {
			Database.Data = new Dictionary<string, string> {
				{ "isloggedin", "false" },
				{ "id", CurrentUserId }
			};
			int id = UserTable.UpdateIsLoggedIn(Database);
			if(id == -1) {
				throw new BadRequestException("Logout failed");
			}
			ResponseContent = new ResponseOk("Logout successful", true);
		}
	}
}
