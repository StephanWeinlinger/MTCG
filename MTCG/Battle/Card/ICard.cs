using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle.Card {
	interface ICard {
		Name Name { get; }
		Type Type { get; }
		Element Element { get; }
		int Damage { get; }
		IDictionary<Element, float> DamageMultiplier { get; }
		int GetActualDamage(ICard enemy);
		int GetSpecialDamage(ICard enemy);
	}
}
