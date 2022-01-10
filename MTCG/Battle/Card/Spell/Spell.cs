using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle.Card.Spell {
	public class Spell : Card {
		public Spell(Element element, int damage) : base(Name.Spell, Type.Spell, element, damage) { }

		public override int GetSpecialDamage(ICard enemy) {
			int damage = -1;
			if(enemy.Name == Name.Kraken) {
				damage = 0;
			}
			return damage;
		}
	}
}
