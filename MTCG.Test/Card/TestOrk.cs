using System.Collections.Generic;
using NUnit.Framework;
using MTCG.Battle;
using MTCG.Battle.Card;
using MTCG.Battle.Card.Monster;
using MTCG.Battle.Card.Spell;
using MTCG.Database.Storage;

namespace MTCG.Test.Card {
    public class TestOrk {
        private Ork _ork;

        [SetUp]
        public void Setup() {
            _ork = new Ork(Element.Normal, 10);
        }

        [Test]
        public void testSpecialDamage_shouldBe0() {
            // Arrange
            Wizard wizard = new Wizard(Element.Normal, 10);
            // Act
            int damage = _ork.GetSpecialDamage(wizard);
            // Assert
            Assert.AreEqual(0, damage);
        }

        [Test]
        public void testSpecialDamage_shouldBeNegative1() {
            // Arrange
            Ork ork = new Ork(Element.Normal, 10);
            // Act
            int damage = _ork.GetSpecialDamage(ork);
            // Assert
            Assert.AreEqual(-1, damage);
        }

    }
}
