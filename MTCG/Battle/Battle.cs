using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Battle.Card;
using MTCG.Database.Storage;

namespace MTCG.Battle {
	public class Battle {
		private Deck _deck1;
		private Deck _deck2;
		public Log.Log Log;

		public Battle(List<CardStorage> deck1, List<CardStorage> deck2) {
			_deck1 = new Deck(deck1);
			_deck2 = new Deck(deck2);
			Log = new Log.Log(_deck1.Owner, _deck2.Owner);
		}

		public void StartBattle() {
			for(int i = 0; i < 100; ++i) {
				ICard card1 = _deck1.GetRandomCard();
				ICard card2 = _deck2.GetRandomCard();
				int specDamage1 = card1.GetSpecialDamage(card2);
				int specDamage2 = card2.GetSpecialDamage(card1);
				int damage1 = card1.GetActualDamage(card2);
				int damage2 = card2.GetActualDamage(card1);
				if(specDamage1 > damage1) {
					damage1 = specDamage1;
				}
				if(specDamage2 > damage2) {
					damage2 = specDamage2;
				}
				// compare damage
				if(damage1 > damage2) {
					_deck1.AddCard(card2);
					_deck2.RemoveLastRandomCard();
					Log.AddEntry(card1, card2, damage1, damage2, false);
				} else if(damage1 < damage2) {
					_deck2.AddCard(card1);
					_deck1.RemoveLastRandomCard();
					Log.AddEntry(card2, card1, damage2, damage1, false);
				} else {
					Log.AddEntry(card2, card1, damage2, damage1, true);
				}

				if(_deck1.IsEmpty) {
					Log.WinnerId = _deck2.Owner;
					Log.LoserId = _deck1.Owner;
					break;
				}
				if(_deck2.IsEmpty) {
					Log.WinnerId = _deck1.Owner;
					Log.LoserId = _deck2.Owner;
					break;
				}
			}
			if(!_deck1.IsEmpty && !_deck2.IsEmpty) {
				Log.IsDraw = true;
			}
		}
	}
}
