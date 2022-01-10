using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle.Card.Monster {
	public class Goblin : Monster{
		public Goblin(Element element, int damage) : base(Name.Goblin, element, damage) { }

		public override int GetSpecialDamage(ICard enemy) {
			int damage = -1;
			if(enemy.Name == Name.Dragon) {
				damage = 0;
			}
			return damage;
		}
	}
}
