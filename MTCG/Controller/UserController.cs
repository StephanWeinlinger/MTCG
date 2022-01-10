using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Database.Storage;
using MTCG.Database.Table;
using MTCG.Exception;
using MTCG.Http;
using MTCG.Http.ResponseContent;
using MTCG.Parse;

namespace MTCG.Controller {
	sealed class UserController : Controller {
		public UserController(HttpRequest request) : base(request) {
			Handle();
		}

		public override void Handle() {
			switch(Request.Method) {
				case HttpMethod.GET:
					CheckAuth();
					GetUser();
					break;
				case HttpMethod.POST:
					InsertUser();
					break;
				case HttpMethod.PUT:
					CheckAuth();
					UpdateUser();
					break;
				default:
					ResponseContent = new ResponseNotFound();
					break;
			}
		}

		// TODO dont get the password
		private void GetUser() {
			UserStorage user = null;
			if(Request.PathContents.Count == 2 && Request.PathContents[1] == CurrentUser.Username) {
				Database.Data = new Dictionary<string, string> {
					{ "username", Request.PathContents[1] }
				};
				user = UserTable.GetUserByUsername(Database);
			}
			if(user == null) {
				throw new BadRequestException("User could not be found");
			}
			ResponseContent = new ResponseOk(JsonSerializer.Serialize(user), false);
		}


		private void InsertUser() {
			Database.Data = JsonDeserializer.Deserialize<Dictionary<string, string>>(Request.Content, DeserializeType.REGISTER_USER);
			Database.Data.Add("token", Database.Data["username"] + "-MTCGToken");
			int id = UserTable.InsertUser(Database);
			if(id == -1) {
				throw new BadRequestException("User could not be inserted");
			}
			Database.Data = new Dictionary<string, string> {
				{ "userid", id.ToString() }
			};
			ScoreboardTable.InsertScoreboard(Database);
			ResponseContent = new ResponseOk("User was inserted", true);
		}

		private void UpdateUser() {
			int id = -1;
			if(Request.PathContents.Count == 2 && Request.PathContents[1] == CurrentUser.Username) {
				Database.Data = JsonDeserializer.Deserialize<Dictionary<string, string>>(Request.Content, DeserializeType.EDIT_USER);
				Database.Data.Add("id", CurrentUserId);
				id = UserTable.UpdateUser(Database);
			}
			if(id == -1) {
				throw new BadRequestException("User could not be updated");
			}
			ResponseContent = new ResponseOk("User was updated", true);
		}
	}
}
