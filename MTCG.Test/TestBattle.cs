using System.Collections.Generic;
using NUnit.Framework;
using MTCG.Battle;
using MTCG.Battle.Card;
using MTCG.Battle.Card.Monster;
using MTCG.Battle.Card.Spell;
using MTCG.Database.Storage;

namespace MTCG.Test {
    public class TestBattle {

        [Test]
        public void testStartBattle_shouldDraw() {
            // Arrange
            var storageCards1 = new List<CardStorage> {
                new CardStorage(1, 1, 10, 1, 1, true, 1),
                new CardStorage(2, 1, 10, 1, 1, true, 1),
                new CardStorage(3, 1, 10, 1, 1, true, 1),
                new CardStorage(4, 1, 10, 1, 1, true, 1)
            };
            var storageCards2 = new List<CardStorage> {
                new CardStorage(1, 2, 10, 1, 1, true, 1),
                new CardStorage(2, 2, 10, 1, 1, true, 1),
                new CardStorage(3, 2, 10, 1, 1, true, 1),
                new CardStorage(4, 2, 10, 1, 1, true, 1)
            };
            var battle = new Battle.Battle(storageCards1, storageCards2);
            // Act
            battle.StartBattle();
            // Assert
            Assert.AreEqual(true, battle.Log.IsDraw);
            Assert.AreEqual(-1, battle.Log.WinnerId);
            Assert.AreEqual(-1, battle.Log.LoserId);
            Assert.AreEqual(100, battle.Log.Rounds.Count);
        }

        [Test]
        public void testStartBattle_Id1ShouldWin() {
            // Arrange
            var storageCards1 = new List<CardStorage> {
                new CardStorage(1, 1, 100, 1, 1, true, 1),
                new CardStorage(2, 1, 100, 1, 1, true, 1),
                new CardStorage(3, 1, 100, 1, 1, true, 1),
                new CardStorage(4, 1, 100, 1, 1, true, 1)
            };
            var storageCards2 = new List<CardStorage> {
                new CardStorage(1, 2, 10, 1, 1, true, 1),
                new CardStorage(2, 2, 10, 1, 1, true, 1),
                new CardStorage(3, 2, 10, 1, 1, true, 1),
                new CardStorage(4, 2, 10, 1, 1, true, 1)
            };
            var battle = new Battle.Battle(storageCards1, storageCards2);
            // Act
            battle.StartBattle();
            // Assert
            Assert.AreEqual(false, battle.Log.IsDraw);
            Assert.AreEqual(1, battle.Log.WinnerId);
            Assert.AreEqual(2, battle.Log.LoserId);
            Assert.Less(battle.Log.Rounds.Count, 100);
        }
    }
}
