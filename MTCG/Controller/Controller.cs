using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MTCG.Database.Storage;
using MTCG.Database.Table;
using MTCG.Http;
using MTCG.Http.ResponseContent;

namespace MTCG.Controller {
	abstract class Controller {
		protected Database.Database Database;
		public HttpRequest Request;
		public ResponseContent ResponseContent;

		protected Controller(HttpRequest request) {
			Request = request;
			Database = new Database.Database();
		}

		protected void CheckAuth() {
			if(Request.Authorization.Length == 0) {
				throw new ArgumentException("User authorization failed");
			}
			var data = new Dictionary<string, string> {
				{ "token", Request.Authorization }
			};
			UserStorage user = UserTable.GetUserByToken(Database, data);
			if(user == null || !user.IsLoggedIn) {
				throw new ArgumentException("User authorization failed");
			}
		}

		public abstract void Handle();
	}
}
