using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle.Card.Monster {
	class Wizard : Monster{
		public Wizard(Element element, int damage) : base(Name.Wizard, element, damage) { }

		public override int GetSpecialDamage(ICard enemy) {
			int damage = -1;
			return damage;
		}
	}
}
