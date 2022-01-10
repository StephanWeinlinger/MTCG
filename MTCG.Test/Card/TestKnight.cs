using System.Collections.Generic;
using NUnit.Framework;
using MTCG.Battle;
using MTCG.Battle.Card;
using MTCG.Battle.Card.Monster;
using MTCG.Battle.Card.Spell;
using MTCG.Database.Storage;

namespace MTCG.Test.Card {
    public class TestKnight {
        private Knight _knight;

        [SetUp]
        public void Setup() {
            _knight = new Knight(Element.Normal, 10);
        }

        [Test]
        public void testSpecialDamage_shouldBe0() {
            // Arrange
            Spell waterSpell = new Spell(Element.Water, 10);
            // Act
            int damage = _knight.GetSpecialDamage(waterSpell);
            // Assert
            Assert.AreEqual(0, damage);
        }

        [Test]
        public void testSpecialDamage_shouldBeNegative1() {
            // Arrange
            Knight knight = new Knight(Element.Normal, 10);
            // Act
            int damage = _knight.GetSpecialDamage(knight);
            // Assert
            Assert.AreEqual(-1, damage);
        }

    }
}
