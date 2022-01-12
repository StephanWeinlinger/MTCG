using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle.Card.Monster {
	public class Dwarf : Monster{
		public Dwarf(Element element, int damage) : base(Name.Dwarf, element, damage) { }

		public override int GetSpecialDamage(ICard enemy) {
			int damage = -1;
			if(enemy.Element != Element.Normal) {
				damage = 0;
			}
			return damage;
		}
	}
}
