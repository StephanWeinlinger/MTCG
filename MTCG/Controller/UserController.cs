using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Database.Storage;
using MTCG.Database.Table;
using MTCG.Http;
using MTCG.Http.ResponseContent;

namespace MTCG.Controller {
	class UserController : Controller {
		public UserController(HttpRequest request) : base(request) { }

		public override void Handle() {
			switch(Request.Method) {
				case HttpMethod.GET:
					GetUser();
					break;
				case HttpMethod.POST:
					InsertUser();
					break;
				case HttpMethod.PUT:
					UpdateUser();
					break;
				default:
					ResponseContent = new ResponseNotFound();
					break;
			}
		}

		private void GetUser() {
			var data = new Dictionary<string, string> {
				{ "token", "admin-MTCGToken" }
			};
			IStorage user = UserTable.GetUserByToken(Database, data);
			if(user == null) {
				ResponseContent = new ResponseBadRequest();
				return;
			}
			ResponseContent = new ResponseOK();
		}


		private void InsertUser() {
			var data = new Dictionary<string, string> {
				{ "username", "esfd" },
				{ "password", "asdf" },
				{ "token", "asfased" }
			};
			int id = UserTable.InsertUser(Database, data);
			if(id == -1) {
				ResponseContent = new ResponseBadRequest();
				return;
			}
			ResponseContent = new ResponseOK();
		}

		private void UpdateUser() {
			var data = new Dictionary<string, string> {
				{ "displayname", "esfd" },
				{ "bio", "asdf" },
				{ "status", "asfased" },
				{ "token", "admin-MTCGToken" }
			};
			int id = UserTable.UpdateUser(Database, data);
			if(id == -1) {
				ResponseContent = new ResponseBadRequest();
				return;
			}
			ResponseContent = new ResponseOK();
		}
	}
}
