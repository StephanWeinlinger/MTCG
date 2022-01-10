using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Http.ResponseContent {
	public class ResponseBadRequest : ResponseContent {
		public ResponseBadRequest() : base(400, "Bad Request") {
			Error = true;
		}

		public ResponseBadRequest(string content, bool isMessage) : base(400, "Bad Request") {
			Error = true;
			SetContent(content, isMessage);
		}
	}
}
