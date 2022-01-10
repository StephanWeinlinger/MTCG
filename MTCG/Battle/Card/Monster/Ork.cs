using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle.Card.Monster {
	public class Ork : Monster{
		public Ork(Element element, int damage) : base(Name.Ork, element, damage) { }

		public override int GetSpecialDamage(ICard enemy) {
			int damage = -1;
			if(enemy.Name == Name.Wizard) {
				damage = 0;
			}
			return damage;
		}
	}
}
