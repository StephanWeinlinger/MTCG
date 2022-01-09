using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle.Card {
	abstract class Card : ICard {
		public Name Name { get; }
		public Type Type { get; }
		public Element Element { get; }
		public int Damage { get; }
		public IDictionary<Element, float> DamageMultiplier { get; set; }

		protected Card(Name name, Type type, Element element, int damage) {
			Name = name;
			Type = type;
			Element = element;
			Damage = damage;
			InitializeDamageMultiplier();
		}

		protected void InitializeDamageMultiplier() {
			float normalMulti = 0;
			float waterMulti = 0;
			float fireMulti = 0;
			switch(Element) {
				case Element.Normal:
					normalMulti = 1;
					waterMulti = 2;
					fireMulti = 0.5f;
					break;
				case Element.Water:
					normalMulti = 0.5f;
					waterMulti = 1;
					fireMulti = 2;
					break;
				case Element.Fire:
					normalMulti = 2;
					waterMulti = 0.5f;
					fireMulti = 1;
					break;
			}
			DamageMultiplier = new Dictionary<Element, float> {
				{ Element.Normal, normalMulti },
				{ Element.Water, waterMulti },
				{ Element.Fire, fireMulti },
			};
		}

		public int GetActualDamage(ICard enemy) {
			int damage = Damage;
			if(Type == Type.Spell || enemy.Type == Type.Spell) {
				switch(enemy.Element) {
					case Element.Normal:
						damage = (int) Math.Round(Damage * DamageMultiplier[Element.Normal], 0);
						break;
					case Element.Water:
						damage = (int) Math.Round(Damage * DamageMultiplier[Element.Water], 0);
						break;
					case Element.Fire:
						damage = (int) Math.Round(Damage * DamageMultiplier[Element.Fire], 0);
						break;
				}
			}
			return damage;
		}

		public abstract int GetSpecialDamage(ICard enemy);
	}
}
