using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle.Card.Monster {
	class Knight : Monster{
		public Knight(Element element, int damage) : base(Name.Knight, element, damage) { }

		public override int GetSpecialDamage(ICard enemy) {
			int damage = -1;
			if(enemy.Name == Name.Spell && enemy.Element == Element.Fire) {
				damage = 0;
			}
			return damage;
		}
	}
}
