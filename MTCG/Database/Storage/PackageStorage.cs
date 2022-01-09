using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Database.Storage {
	class PackageStorage : IStorage {
		public PackageStorage(int id, int card1, int card2, int card3, int card4, int card5) {
			Id = id;
			Card1 = card1;
			Card2 = card2;
			Card3 = card3;
			Card4 = card4;
			Card5 = card5;
		}
		public int Id;
		public int Card1;
		public int Card2;
		public int Card3;
		public int Card4;
		public int Card5;
	}
}
