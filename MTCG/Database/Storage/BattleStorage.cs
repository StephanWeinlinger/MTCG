using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Database.Storage {
	public class BattleStorage : IStorage {
		public BattleStorage(int id, int user1, int? user2) {
			Id = id;
			User1 = user1;
			User2 = user2;
		}
		public int Id;
		public int User1;
		public int? User2;
	}
}
