using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Database.Storage;
using NpgsqlTypes;

namespace MTCG.Database.Table {
	static class BattleTable {

		public static List<BattleStorage> GetAllBattles(Database db) {
			db.Statement = "SELECT * FROM \"battle\"";
			db.Fields = new Dictionary<string, NpgsqlDbType>();
			db.PrepareCommand();
			var battles = ReadBattles(db.ExecuteCommandWithRead());
			return battles;
		}

		public static int InsertBattle(Database db) {
			db.Statement = "INSERT INTO \"battle\" (user1) VALUES (@user1) RETURNING id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "user1", NpgsqlDbType.Integer}
			};
			db.PrepareCommand();
			int id = ReadId(db.ExecuteCommandWithRead());
			return id;
		}

		public static int UpdateBattle(Database db) {
			db.Statement = "UPDATE \"battle\" SET user2 = @user2 WHERE id = @id RETURNING id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "user2", NpgsqlDbType.Integer},
				{ "id", NpgsqlDbType.Integer}
			};
			db.PrepareCommand();
			int id = ReadId(db.ExecuteCommandWithRead());
			return id;
		}

		public static bool DeleteBattle(Database db) {
			db.Statement = "DELETE FROM \"battle\" WHERE id = @id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "id", NpgsqlDbType.Integer}
			};
			db.PrepareCommand();
			bool error = db.ExecuteCommandWithoutRead();
			return error;
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

		private static List<BattleStorage> ReadBattles(IDataReader reader) {
			List<BattleStorage> battles = new List<BattleStorage>();
			if(reader != null) {
				while(reader.Read()) {
					int? user2 = !reader.IsDBNull(2) ? reader.GetInt32(2) : null;
					battles.Add(new BattleStorage(
						reader.GetInt32(0),
						reader.GetInt32(1),
						user2
					));
				}
				reader.Close();
			}
			return battles;
		}
	}
}
