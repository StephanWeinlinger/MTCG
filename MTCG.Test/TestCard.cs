using System.Collections.Generic;
using NUnit.Framework;
using MTCG.Battle;
using MTCG.Battle.Card;
using MTCG.Battle.Card.Monster;
using MTCG.Battle.Card.Spell;
using MTCG.Database.Storage;

namespace MTCG.Test {
    public class TestCard {

        [Test]
        public void testGetActualDamage_shouldBeNormalWithoutElement() {
            // Arrange
            Goblin normalGoblin = new Goblin(Element.Normal, 10);
            Goblin waterGoblin = new Goblin(Element.Water, 10);
            // Act
            int damage = normalGoblin.GetActualDamage(waterGoblin);
            // Assert
            Assert.AreEqual(10, damage);
        }

        [Test]
        public void testGetActualDamage_shouldBeNormal() {
            // Arrange
            Goblin normalGoblin = new Goblin(Element.Normal, 10);
            Spell normalSpell = new Spell(Element.Normal, 10);
            // Act
            int damage = normalGoblin.GetActualDamage(normalSpell);
            // Assert
            Assert.AreEqual(10, damage);
        }

        [Test]
        public void testGetActualDamage_shouldBeDouble() {
            // Arrange
            Goblin normalGoblin = new Goblin(Element.Normal, 10);
            Spell waterSpell = new Spell(Element.Water, 10);
            // Act
            int damage = normalGoblin.GetActualDamage(waterSpell);
            // Assert
            Assert.AreEqual(20, damage);
        }

        [Test]
        public void testGetActualDamage_shouldBeHalved() {
            // Arrange
            Goblin normalGoblin = new Goblin(Element.Normal, 10);
            Spell fireSpell = new Spell(Element.Fire, 10);
            // Act
            int damage = normalGoblin.GetActualDamage(fireSpell);
            // Assert
            Assert.AreEqual(5, damage);
        }
    }
}
