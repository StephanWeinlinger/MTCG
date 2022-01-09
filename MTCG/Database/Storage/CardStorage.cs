using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Database.Storage {
	class CardStorage : IStorage {
		public CardStorage(int id, int? owner, int damage, int type, int element, bool inDeck, int name) {
			Id = id;
			Owner = owner;
			Damage = damage;
			Type = type;
			Element = element;
			InDeck = inDeck;
			Name = name;
		}
		public int Id;
		public int? Owner;
		public int Damage;
		public int Type;
		public int Element;
		public bool InDeck;
		public int Name;
	}
}
