using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Database.Storage;
using NpgsqlTypes;

namespace MTCG.Database.Table {
	public static class TradeTable {

		public static List<JoinedTradeStorage> GetAllTradesJoined(Database db) {
			db.Statement = "SELECT tr.id, tr.cardid, n.text, t.text, e.text, c.damage, tw.text, ew.text, tr.mindamage, tr.seller FROM ((((((\"trade\" tr JOIN \"card\" c ON tr.cardid = c.id) JOIN \"name\" n ON c.name = n.id) JOIN \"type\" t ON c.type = t.id) JOIN \"element\" e ON c.element = e.id) LEFT JOIN \"type\" tw ON tr.wantedtype = tw.id) LEFT JOIN \"element\" ew ON tr.wantedelement = ew.id)";
			db.Fields = new Dictionary<string, NpgsqlDbType>();
			db.PrepareCommand();
			var trades = ReadJoinedTrades(db.ExecuteCommandWithRead());
			return trades;
		}

		public static TradeStorage GetTrade(Database db) {
			db.Statement = "SELECT * FROM \"trade\" WHERE id = @id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "id", NpgsqlDbType.Integer }
			};
			db.PrepareCommand();
			var trade = ReadTrade(db.ExecuteCommandWithRead());
			return trade;
		}

		public static bool DeleteTrade(Database db) {
			db.Statement = "DELETE FROM \"trade\" WHERE id = @id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "id", NpgsqlDbType.Integer }
			};
			db.PrepareCommand();
			bool error = db.ExecuteCommandWithoutRead();
			return error;
		}

		public static int InsertTrade(Database db) {
			db.Statement = "INSERT INTO \"trade\" (cardid, wantedtype, wantedelement, mindamage, seller) VALUES (@cardid, @wantedtype, @wantedelement, @mindamage, @seller) RETURNING id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "cardid", NpgsqlDbType.Integer },
				{ "wantedtype", NpgsqlDbType.Integer },
				{ "wantedelement", NpgsqlDbType.Integer },
				{ "mindamage", NpgsqlDbType.Integer },
				{ "seller", NpgsqlDbType.Integer }
			};
			db.PrepareCommand();
			int id = ReadId(db.ExecuteCommandWithRead());
			return id;
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

		private static TradeStorage ReadTrade(IDataReader reader) {
			TradeStorage trade = null;
			if(reader != null) {
				if(reader.Read()) {
					int? wantedType = !reader.IsDBNull(2) ? reader.GetInt32(2) : null;
					int? wantedElement = !reader.IsDBNull(3) ? reader.GetInt32(3) : null;
					trade = new TradeStorage(
						reader.GetInt32(0),
						reader.GetInt32(1),
						wantedType,
						wantedElement,
						reader.GetInt32(4),
						reader.GetInt32(5)
					);

				}
				reader.Close();
			}
			return trade;
		}

		private static List<JoinedTradeStorage> ReadJoinedTrades(IDataReader reader) {
			var trades = new List<JoinedTradeStorage>();
			if(reader != null) {
				while(reader.Read()) {
					string wantedType = !reader.IsDBNull(6) ? reader.GetString(6) : null;
					string wantedElement = !reader.IsDBNull(7) ? reader.GetString(7) : null;
					trades.Add(new JoinedTradeStorage(
						reader.GetInt32(0),
						reader.GetInt32(1),
						reader.GetString(2),
						reader.GetString(3),
						reader.GetString(4),
						reader.GetInt32(5),
						wantedType,
						wantedElement,
						reader.GetInt32(8),
						reader.GetInt32(9)
					));

				}
				reader.Close();
			}
			return trades;
		}
	}
}
