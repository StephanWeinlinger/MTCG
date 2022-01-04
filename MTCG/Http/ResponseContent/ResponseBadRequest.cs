using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Http.ResponseContent {
	class ResponseBadRequest : ResponseContent {
		public ResponseBadRequest() : base(400, "Bad Request") {}
	}
}
