using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Battle.Card;
using MTCG.Battle.Card.Monster;
using MTCG.Battle.Card.Spell;
using MTCG.Database.Storage;

namespace MTCG.Battle {
	public class Deck {
		public int Owner { get; private set; }
		public IList<ICard> Cards;
		private int _lastIndex;
		public bool IsEmpty;

		public Deck(IList<CardStorage> storageCards) {
			Cards = new List<ICard>();
			_lastIndex = -1;
			IsEmpty = false;
			Owner = (int) storageCards[0].Owner;
			foreach(var entry in storageCards) {
				Name name = (Name) entry.Name;
				Element element = (Element) entry.Element;
				switch(name) {
					case Name.Goblin:
						Cards.Add(new Goblin(element, entry.Damage));
						break;
					case Name.Wizard:
						Cards.Add(new Wizard(element, entry.Damage));
						break;
					case Name.Dragon:
						Cards.Add(new Dragon(element, entry.Damage));
						break;
					case Name.Ork:
						Cards.Add(new Ork(element, entry.Damage));
						break;
					case Name.Knight:
						Cards.Add(new Knight(element, entry.Damage));
						break;
					case Name.Kraken:
						Cards.Add(new Kraken(element, entry.Damage));
						break;
					case Name.Elve:
						Cards.Add(new Elve(element, entry.Damage));
						break;
					case Name.Spell:
						Cards.Add(new Spell(element, entry.Damage));
						break;
				}
			}
		}

		public ICard GetRandomCard() {
			_lastIndex = Randomizer.GetNumber(Cards.Count);
			return Cards[_lastIndex];
		}

		public void AddCard(ICard newCard) {
			Cards.Add(newCard);
		}

		public void RemoveLastRandomCard() {
			if(_lastIndex != -1) {
				Cards.RemoveAt(_lastIndex);
				_lastIndex = -1;
			}
			if(Cards.Count == 0) {
				IsEmpty = true;
			}
		}
	}
}
