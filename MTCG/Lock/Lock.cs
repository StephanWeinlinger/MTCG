﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Lock {
	static class Lock {
		public static Object PackageLocker = new Object();
	}
}
