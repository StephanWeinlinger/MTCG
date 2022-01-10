using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Http.ResponseContent {
	public class ResponseUnauthorized : ResponseContent {
		public ResponseUnauthorized() : base(401, "Unauthorized") {
			Error = true;
		}

		public ResponseUnauthorized(string content, bool isMessage) : base(401, "Unauthorized") {
			Error = true;
			SetContent(content,isMessage);
		}
	}
}
