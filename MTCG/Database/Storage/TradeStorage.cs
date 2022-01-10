using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Database.Storage {
	class TradeStorage : IStorage {
		public TradeStorage(int id, int cardId, int? wantedType, int? wantedElement, int minDamage, int seller) {
			Id = id;
			CardId = cardId;
			WantedType = wantedType;
			WantedElement = wantedElement;
			MinDamage = minDamage;
			Seller = seller;
		}
		public int Id;
		public int CardId;
		public int? WantedType;
		public int? WantedElement;
		public int MinDamage;
		public int Seller;
	}
}
