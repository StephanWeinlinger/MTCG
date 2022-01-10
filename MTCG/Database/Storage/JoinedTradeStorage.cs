using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Database.Storage {
	public class JoinedTradeStorage : IStorage {
		public JoinedTradeStorage(int id, int cardId, string cardName, string cardType, string cardElement, int cardDamage, string wantedType, string wantedElement, int minDamage, int seller) {
			Id = id;
			CardId = cardId;
			CardName = cardName;
			CardType = cardType;
			CardElement = cardElement;
			CardDamage = cardDamage;
			WantedType = wantedType;
			WantedElement = wantedElement;
			MinDamage = minDamage;
			Seller = seller;
		}
		public int Id;
		public int CardId;
		public string CardName;
		public string CardType;
		public string CardElement;
		public int CardDamage;
		public string WantedType;
		public string WantedElement;
		public int MinDamage;
		public int Seller;
	}
}
