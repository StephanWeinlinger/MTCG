using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle.Card.Monster {
	public class Skeleton : Monster{
		public Skeleton(Element element, int damage) : base(Name.Skeleton, element, damage) { }

		public override int GetSpecialDamage(ICard enemy) {
			int damage = -1;
			if(enemy.Element == Element.Fire) {
				damage = 30;
			}
			return damage;
		}
	}
}
