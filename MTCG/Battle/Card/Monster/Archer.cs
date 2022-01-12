using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle.Card.Monster {
	public class Archer : Monster{
		public Archer(Element element, int damage) : base(Name.Archer, element, damage) { }

		public override int GetSpecialDamage(ICard enemy) {
			int damage = -1;
			if(enemy.Name == Name.Dragon) {
				damage = 40;
			}
			return damage;
		}
	}
}
