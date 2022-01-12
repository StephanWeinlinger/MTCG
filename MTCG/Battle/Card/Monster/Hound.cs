using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle.Card.Monster {
	public class Hound : Monster{
		public Hound(Element element, int damage) : base(Name.Hound, element, damage) { }

		public override int GetSpecialDamage(ICard enemy) {
			int damage = -1;
			if(enemy.Name == Name.Knight || enemy.Name == Name.Archer) {
				damage = 0;
			}
			return damage;
		}
	}
}
