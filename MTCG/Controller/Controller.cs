using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public abstract void Handle();
	}
}
