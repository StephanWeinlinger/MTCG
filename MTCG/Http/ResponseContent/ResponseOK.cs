using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Http.ResponseContent {
	class ResponseOK : ResponseContent {
		public ResponseOK() : base(200, "OK") {}
	}
}
