using System.Collections.Generic;
using NUnit.Framework;
using MTCG.Battle;
using MTCG.Battle.Card;
using MTCG.Battle.Card.Monster;
using MTCG.Database.Storage;

namespace MTCG.Test.Card {
    public class TestDragon {
        private Dragon _dragon;

        [SetUp]
        public void Setup() {
            _dragon = new Dragon(Element.Normal, 10);
        }

        [Test]
        public void testSpecialDamage_shouldBe0() {
            // Arrange
            Elve fireElve = new Elve(Element.Fire, 10);
            // Act
            int damage = _dragon.GetSpecialDamage(fireElve);
            // Assert
            Assert.AreEqual(0, damage);
        }

        [Test]
        public void testSpecialDamage_shouldBeNegative1() {
            // Arrange
            Dragon dragon = new Dragon(Element.Normal, 10);
            // Act
            int damage = _dragon.GetSpecialDamage(dragon);
            // Assert
            Assert.AreEqual(-1, damage);
        }

    }
}
