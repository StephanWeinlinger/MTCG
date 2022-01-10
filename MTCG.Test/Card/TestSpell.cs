using System.Collections.Generic;
using NUnit.Framework;
using MTCG.Battle;
using MTCG.Battle.Card;
using MTCG.Battle.Card.Monster;
using MTCG.Battle.Card.Spell;
using MTCG.Database.Storage;

namespace MTCG.Test.Card {
    public class TestSpell {
        private Spell _spell;

        [SetUp]
        public void Setup() {
            _spell = new Spell(Element.Normal, 10);
        }

        [Test]
        public void testSpecialDamage_shouldBe0() {
            // Arrange
            Kraken kraken = new Kraken(Element.Normal, 10);
            // Act
            int damage = _spell.GetSpecialDamage(kraken);
            // Assert
            Assert.AreEqual(0, damage);
        }

        [Test]
        public void testSpecialDamage_shouldBeNegative1() {
            // Arrange
            Spell spell = new Spell(Element.Normal, 10);
            // Act
            int damage = _spell.GetSpecialDamage(spell);
            // Assert
            Assert.AreEqual(-1, damage);
        }

    }
}
