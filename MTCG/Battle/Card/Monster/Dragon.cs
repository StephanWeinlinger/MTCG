using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle.Card.Monster {
	class Dragon : Monster{
		public Dragon(Element element, int damage) : base(Name.Dragon, element, damage) { }

		public override int GetSpecialDamage(ICard enemy) {
			int damage = -1;
			if(enemy.Name == Name.Elve && enemy.Element == Element.Fire) {
				damage = 0;
			}
			return damage;
		}
	}
}
