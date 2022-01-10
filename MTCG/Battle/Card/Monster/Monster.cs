using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle.Card.Monster {
	public abstract class Monster : Card {
		protected Monster(Name name, Element element, int damage) : base(name, Type.Monster, element, damage) { }
	}
}
