using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Lock {
	public static class Lock {
		public static Object PackageLocker = new Object();
		public static Object BattleLocker = new Object();
		public static Object TradeInsertLocker = new Object();
		public static Object TradeDoLocker = new Object();
	}
}
