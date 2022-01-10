using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Http.ResponseContent {
	public class ResponseNotFound : ResponseContent {
		public ResponseNotFound() : base(404, "Not Found") {
			Error = true;
			SetContent("Endpoint was not found", true);
		}
	}
}
