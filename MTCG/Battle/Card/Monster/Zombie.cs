using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle.Card.Monster {
	public class Zombie : Monster{
		public Zombie(Element element, int damage) : base(Name.Zombie, element, damage) { }

		public override int GetSpecialDamage(ICard enemy) {
			int damage = -1;
			if(enemy.Name == Name.Knight && enemy.Element == Element.Normal) {
				damage = 50;
			}
			return damage;
		}
	}
}
