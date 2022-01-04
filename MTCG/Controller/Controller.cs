using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Controller {
	abstract class Controller {
		//private IDbConnection _connection;

		protected Controller() {
			
		}

		protected abstract void Handle();
	}
}
