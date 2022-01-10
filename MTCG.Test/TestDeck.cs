using System.Collections.Generic;
using NUnit.Framework;
using MTCG.Battle;
using MTCG.Battle.Card;
using MTCG.Battle.Card.Monster;
using MTCG.Battle.Card.Spell;
using MTCG.Database.Storage;

namespace MTCG.Test {
    public class TestDeck {
        private Deck _deck;

        [SetUp]
        public void Setup() {
            var storageCards = new List<CardStorage> {
                new CardStorage(1, 1, 1, 1, 2, true, 1),
                new CardStorage(2, 1, 2, 1, 3, true, 3),
                new CardStorage(3, 1, 3, 1, 1, true, 6),
                new CardStorage(4, 1, 8, 2, 3, true, 8)
            };
            _deck = new Deck(storageCards);
        }

        [Test]
        public void testConstructor_shouldInitializeCorrectClasses() {
            // Arrange
            var storageCards = new List<CardStorage> {
                new CardStorage(1, 1, 1, 1, 2, true, 1),
                new CardStorage(2, 1, 2, 1, 3, true, 3),
                new CardStorage(3, 1, 3, 1, 1, true, 6),
                new CardStorage(4, 1, 8, 2, 3, true, 8)
            };
            var deck = new Deck(storageCards);
            // Assert
            Assert.IsInstanceOf(typeof(Goblin), _deck.Cards[0]);
            Assert.IsInstanceOf(typeof(Dragon), _deck.Cards[1]);
            Assert.IsInstanceOf(typeof(Kraken), _deck.Cards[2]);
            Assert.IsInstanceOf(typeof(Spell), _deck.Cards[3]);
        }

        [Test]
        public void testGetRandomCard_shouldReturnCardFromCards() {
            // Arrange
            var randomCard = _deck.GetRandomCard();
            // Assert
            if (_deck.Cards.Contains(randomCard)) {
                Assert.Pass();
            } else {
                Assert.Fail();
            }
        }

        [Test]
        public void testRemoveLastRandomCard_shouldRemoveLastRandomCard() {
            // Arrange
            var randomCard = _deck.GetRandomCard();
            // Act
            _deck.RemoveLastRandomCard();
            // Assert
            if (_deck.Cards.Contains(randomCard)) {
                Assert.Fail();
            } else {
                Assert.Pass();
            }
        }

        [Test]
        public void testRemoveLastRandomCard_shouldContain3Cards() {
            // Arrange
            var randomCard = _deck.GetRandomCard();
            // Act
            _deck.RemoveLastRandomCard();
            // Assert
            Assert.AreEqual(3, _deck.Cards.Count);
        }

        [Test]
        public void testRemoveLastRandomCard_shouldSetIsEmptyTrue() {
            // Act
            for (int i = 0; i < 4; ++i) {
                _deck.GetRandomCard();
                _deck.RemoveLastRandomCard();
            }
            // Assert
            Assert.AreEqual(true, _deck.IsEmpty);
        }

        [Test]
        public void testRemoveLastRandomCard_shouldNotRemoveCard() {
            // Act
            _deck.RemoveLastRandomCard();
            // Assert
            Assert.AreEqual(4, _deck.Cards.Count);
        }

        [Test]
        public void testAddCard_shouldAddCard() {
            // Arrange
            var card = new Goblin(Element.Fire, 20);
            // Act
            _deck.AddCard(card);
            // Assert
            Assert.AreEqual(5, _deck.Cards.Count);
        }
    }
}
