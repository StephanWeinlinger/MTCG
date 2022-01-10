using System.Collections.Generic;
using NUnit.Framework;
using MTCG.Battle;
using MTCG.Battle.Card;
using MTCG.Battle.Card.Monster;
using MTCG.Battle.Card.Spell;
using MTCG.Database.Storage;

namespace MTCG.Test.Card {
    public class TestGoblin {
        private Goblin _goblin;

        [SetUp]
        public void Setup() {
            _goblin = new Goblin(Element.Normal, 10);
        }

        [Test]
        public void testSpecialDamage_shouldBe0() {
            // Arrange
            Dragon dragon = new Dragon(Element.Normal, 10);
            // Act
            int damage = _goblin.GetSpecialDamage(dragon);
            // Assert
            Assert.AreEqual(0, damage);
        }

        [Test]
        public void testSpecialDamage_shouldBeNegative1() {
            // Arrange
            Goblin goblin = new Goblin(Element.Normal, 10);
            // Act
            int damage = _goblin.GetSpecialDamage(goblin);
            // Assert
            Assert.AreEqual(-1, damage);
        }
    }
}
