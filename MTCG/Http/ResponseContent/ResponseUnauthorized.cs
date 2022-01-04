using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Http.ResponseContent {
	class ResponseUnauthorized : ResponseContent {
		public ResponseUnauthorized() : base(401, "Unauthorized") {}
	}
}
