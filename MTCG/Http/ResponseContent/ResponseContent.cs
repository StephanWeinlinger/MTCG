using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Http.ResponseContent {
	class ResponseContent {
		// DON'T FORGET TO SERIALIZE EXTERNALLY
		public string Content { get; set; }
		public int Code { get; private set; }
		public string Message { get; private set; }

		protected ResponseContent(int code, string message) {
			Code = code;
			Message = message;
			Content = "";
		}
	}
}
