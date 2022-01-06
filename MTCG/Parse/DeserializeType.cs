using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Parse {
	public enum DeserializeType {
		REGISTER_USER,
		LOGIN_USER,
		CREATE_PACKAGE,
		CONFIGURE_DECK,
		EDIT_USER,
		CREATE_TRADE,
		DO_TRADE
	}
}
