using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Database.Storage;
using NpgsqlTypes;

namespace MTCG.Database.Table {
	static class CardTable {

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

		public static List<CardStorage> GetAllCardsInDeck(Database db) {
			db.Statement = "SELECT c.id, c.owner, c.damage, t.text, e.text, c.indeck, n.text FROM (((\"card\" c JOIN \"type\" t on c.type = t.id) JOIN \"element\" e on c.element = e.id) JOIN \"name\" n on c.name = n.id) WHERE c.owner = @owner AND c.indeck = true";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "owner", NpgsqlDbType.Integer },
			};
			db.PrepareCommand();
			var cards = ReadCards(db.ExecuteCommandWithRead());
			return cards;
		}

		private static int ReadId(IDataReader reader) {
			int id;
			if(reader != null && reader.Read()) {
				id = reader.GetInt32(0);
				reader.Close();
			} else {
				id = -1;
			}
			return id;
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
