using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Http.ResponseContent {
	public class ResponseOk : ResponseContent {
		public ResponseOk() : base(200, "OK") {
			Error = false;
		}

		public ResponseOk(string content, bool isMessage) : base(200, "OK") {
			Error = false;
			SetContent(content,isMessage);
		}
	}
}
