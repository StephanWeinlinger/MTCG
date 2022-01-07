using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Database.Storage;
using MTCG.Database.Table;
using MTCG.Http;
using MTCG.Http.ResponseContent;
using MTCG.Parse;

namespace MTCG.Controller {
	class UserController : Controller {
		public UserController(HttpRequest request) : base(request) { }

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

		private void GetUser() {
			UserStorage user = null;
			if(Request.PathContents.Count == 2 && Request.PathContents[1] == CurrentUser.Username) {
				var data = new Dictionary<string, string> {
					{ "username", Request.PathContents[1] }
				};
				user = UserTable.GetUserByUsername(Database, data);
			}
			if(user == null) {
				ResponseContent = new ResponseBadRequest();
				return;
			}
			ResponseContent = new ResponseOK();
		}


		private void InsertUser() {
			var data = JsonDeserializer.Deserialize<Dictionary<string, string>>(Request.Content, DeserializeType.REGISTER_USER);
			data.Add("token", data["username"] + "-MTCGToken");
			int id = UserTable.InsertUser(Database, data);
			if(id == -1) {
				ResponseContent = new ResponseBadRequest();
				return;
			}
			ResponseContent = new ResponseOK();
		}

		private void UpdateUser() {
			int id = -1;
			if(Request.PathContents.Count == 2 && Request.PathContents[1] == CurrentUser.Username) {
				var data = JsonDeserializer.Deserialize<Dictionary<string, string>>(Request.Content, DeserializeType.EDIT_USER);
				data.Add("id", CurrentUserId);
				id = UserTable.UpdateUser(Database, data);
			}
			if(id == -1) {
				ResponseContent = new ResponseBadRequest();
				return;
			}
			ResponseContent = new ResponseOK();
		}
	}
}
