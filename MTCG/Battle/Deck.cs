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
	class Deck {
		public int Owner { get; private set; }
		private IList<ICard> _cards;
		private int _lastIndex;
		public bool IsEmpty;

		public Deck(IList<CardStorage> storageCards) {
			_cards = new List<ICard>();
			_lastIndex = -1;
			IsEmpty = false;
			Owner = (int) storageCards[0].Owner;
			foreach(var entry in storageCards) {
				Name name = (Name) entry.Name;
				Element element = (Element) entry.Element;
				switch(name) {
					case Name.Goblin:
						_cards.Add(new Goblin(element, entry.Damage));
						break;
					case Name.Wizard:
						_cards.Add(new Wizard(element, entry.Damage));
						break;
					case Name.Dragon:
						_cards.Add(new Dragon(element, entry.Damage));
						break;
					case Name.Ork:
						_cards.Add(new Ork(element, entry.Damage));
						break;
					case Name.Knight:
						_cards.Add(new Knight(element, entry.Damage));
						break;
					case Name.Kraken:
						_cards.Add(new Kraken(element, entry.Damage));
						break;
					case Name.Elve:
						_cards.Add(new Elve(element, entry.Damage));
						break;
					case Name.Spell:
						_cards.Add(new Spell(element, entry.Damage));
						break;
				}
			}
		}

		public ICard GetRandomCard() {
			_lastIndex = Randomizer.GetNumber(_cards.Count);
			return _cards[_lastIndex];
		}

		public void AddCard(ICard newCard) {
			_cards.Add(newCard);
		}

		public void RemoveLastRandomCard() {
			if(_lastIndex != -1) {
				_cards.RemoveAt(_lastIndex);
			}
			if(_cards.Count == 0) {
				IsEmpty = true;
			}
		}
	}
}
