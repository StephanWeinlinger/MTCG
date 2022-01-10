using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Database.Storage {
	class JoinedCardStorage : IStorage {
		public JoinedCardStorage(int id, int? owner, int damage, string type, string element, bool inDeck, string name) {
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
		public string Type;
		public string Element;
		public bool InDeck;
		public string Name;
	}
}
