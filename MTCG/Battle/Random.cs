using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle {
	public static class Randomizer {
		private static Random _random = new Random();

		public static int GetNumber(int maxValue) {
			return _random.Next(maxValue);
		}
	}
}
