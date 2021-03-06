using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Database.Storage;
using NpgsqlTypes;

namespace MTCG.Database.Table {
	public static class CardTable {

		public static int InsertCard(Database db) {
			db.Statement = "INSERT INTO \"card\" (name, type, element, damage) VALUES (@name, @type, @element, @damage) RETURNING id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "name", NpgsqlDbType.Integer },
				{ "type", NpgsqlDbType.Integer },
				{ "element", NpgsqlDbType.Integer },
				{ "damage", NpgsqlDbType.Integer },
			};
			db.PrepareCommand();
			int id = ReadId(db.ExecuteCommandWithRead());
			return id;
		}

		public static int UpdateCard(Database db) {
			db.Statement = "UPDATE \"card\" SET owner = @owner WHERE id = @id RETURNING id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "owner", NpgsqlDbType.Integer },
				{ "id", NpgsqlDbType.Integer }
			};
			db.PrepareCommand();
			int id = ReadId(db.ExecuteCommandWithRead());
			return id;
		}

		public static CardStorage GetCard(Database db) {
			db.Statement = "SELECT * FROM \"card\" WHERE id = @id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "id", NpgsqlDbType.Integer },
			};
			db.PrepareCommand();
			var card = ReadCard(db.ExecuteCommandWithRead());
			return card;
		}

		public static List<CardStorage> GetAllCardsInDeck(Database db) {
			db.Statement = "SELECT * FROM \"card\" WHERE owner = @owner AND indeck = true";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "owner", NpgsqlDbType.Integer },
			};
			db.PrepareCommand();
			var cards = ReadCards(db.ExecuteCommandWithRead());
			return cards;
		}

		public static List<JoinedCardStorage> GetAllCardsFromUserJoined(Database db) {
			db.Statement = "SELECT c.id, c.owner, c.damage, t.text, e.text, c.indeck, n.text FROM (((\"card\" c JOIN \"type\" t on c.type = t.id) JOIN \"element\" e on c.element = e.id) JOIN \"name\" n on c.name = n.id) WHERE c.owner = @owner";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "owner", NpgsqlDbType.Integer },
			};
			db.PrepareCommand();
			var cards = ReadJoinedCards(db.ExecuteCommandWithRead());
			return cards;
		}

		public static List<JoinedCardStorage> GetAllCardsInDeckJoined(Database db) {
			db.Statement = "SELECT c.id, c.owner, c.damage, t.text, e.text, c.indeck, n.text FROM (((\"card\" c JOIN \"type\" t on c.type = t.id) JOIN \"element\" e on c.element = e.id) JOIN \"name\" n on c.name = n.id) WHERE c.owner = @owner AND c.indeck = true";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "owner", NpgsqlDbType.Integer },
			};
			db.PrepareCommand();
			var cards = ReadJoinedCards(db.ExecuteCommandWithRead());
			return cards;
		}

		public static bool UpdateAllCardsOldDeck(Database db) {
			db.Statement = "UPDATE \"card\" SET indeck = false WHERE owner = @owner AND indeck = true";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "owner", NpgsqlDbType.Integer },
			};
			db.PrepareCommand();
			bool error = db.ExecuteCommandWithoutRead();
			return error;
		}

		public static bool UpdateCardNewDeck(Database db) {
			db.Statement = "UPDATE \"card\" SET indeck = true WHERE id = @id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "id", NpgsqlDbType.Integer },
			};
			db.PrepareCommand();
			bool error = db.ExecuteCommandWithoutRead();
			return error;
		}

		private static int ReadId(IDataReader reader) {
			int id = -1;
			if(reader != null) {
				if(reader.Read()) {
					id = reader.GetInt32(0);
				}
				reader.Close();
			}
			return id;
		}

		private static CardStorage ReadCard(IDataReader reader) {
			CardStorage card = null;
			if(reader != null) {
				if(reader.Read()) {
					int? owner = !reader.IsDBNull(1) ? reader.GetInt32(1) : null;
					card = new CardStorage(
						reader.GetInt32(0),
						owner,
						reader.GetInt32(2),
						reader.GetInt32(3),
						reader.GetInt32(4),
						reader.GetBoolean(5),
						reader.GetInt32(6)
					);
				}
				reader.Close();
			}
			return card;
		}

		private static List<CardStorage> ReadCards(IDataReader reader) {
			List<CardStorage> cards = new List<CardStorage>();
			if(reader != null) {
				while(reader.Read()) {
					int? owner = !reader.IsDBNull(1) ? reader.GetInt32(1) : null;
					cards.Add(new CardStorage(
						reader.GetInt32(0),
						owner,
						reader.GetInt32(2),
						reader.GetInt32(3),
						reader.GetInt32(4),
						reader.GetBoolean(5),
						reader.GetInt32(6)
					));
				}
				reader.Close();
			}
			return cards;
		}

		private static List<JoinedCardStorage> ReadJoinedCards(IDataReader reader) {
			List<JoinedCardStorage> cards = new List<JoinedCardStorage>();
			if(reader != null) {
				while(reader.Read()) {
					int? owner = !reader.IsDBNull(1) ? reader.GetInt32(1) : null;
					cards.Add(new JoinedCardStorage(
						reader.GetInt32(0),
						owner,
						reader.GetInt32(2),
						reader.GetString(3),
						reader.GetString(4),
						reader.GetBoolean(5),
						reader.GetString(6)
					));
				}
				reader.Close();
			}
			return cards;
		}
	}
}
