using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MTCG.Database.Storage;
using MTCG.Database.Table;
using MTCG.Exception;
using MTCG.Http;
using MTCG.Http.ResponseContent;

namespace MTCG.Controller {
	public abstract class Controller {
		protected Database.Database Database;
		public HttpRequest Request;
		public ResponseContent ResponseContent;
		protected UserStorage CurrentUser;
		protected string CurrentUserId;

		protected Controller(HttpRequest request) {
			Request = request;
			Database = new Database.Database();
			CurrentUserId = null;
		}

		protected void CheckAuth() {
			if(Request.Authorization.Length == 0) {
				throw new FailedAuthException("User authorization failed");
			}
			Database.Data = new Dictionary<string, string> {
				{ "token", Request.Authorization }
			};
			CurrentUser = UserTable.GetUserByToken(Database);
			if(CurrentUser == null || !CurrentUser.IsLoggedIn) {
				throw new FailedAuthException("User authorization failed");
			}
			CurrentUserId = CurrentUser.Id.ToString();
		}

		public abstract void Handle();
	}
}
