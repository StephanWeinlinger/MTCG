﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Exception {
	class BadRequestException : System.Exception {
		public BadRequestException(string message)
			: base(message) {
		}
	}
}