using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Exception {
	public class FailedAuthException : System.Exception {
		public FailedAuthException(string message)
			: base(message) {
		}
	}
}
