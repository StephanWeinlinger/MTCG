using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Database.Storage;
using NpgsqlTypes;

namespace MTCG.Database.Table {
	static class ScoreboardTable {

		public static ScoreboardStorage GetScoreboard(Database db) {
			db.Statement = "SELECT * FROM \"scoreboard\" WHERE userid = @userid";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "userid", NpgsqlDbType.Integer },
			};
			db.PrepareCommand();
			ScoreboardStorage scoreboard = ReadScoreboard(db.ExecuteCommandWithRead());
			return scoreboard;
		}

		public static int InsertScoreboard(Database db) {
			db.Statement = "INSERT INTO \"scoreboard\" (userid) VALUES (@userid) RETURNING userid";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "userid", NpgsqlDbType.Integer },
			};
			db.PrepareCommand();
			int id = ReadId(db.ExecuteCommandWithRead());
			return id;
		}

		public static int UpdateScoreboard(Database db) {
			db.Statement = "UPDATE \"scoreboard\" SET elo = @elo, wins = @wins, losses = @losses, draws = @draws WHERE userid = @userid";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "elo", NpgsqlDbType.Integer },
				{ "wins", NpgsqlDbType.Integer },
				{ "losses", NpgsqlDbType.Integer },
				{ "draws", NpgsqlDbType.Integer },
				{ "userid", NpgsqlDbType.Integer },
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

		private static ScoreboardStorage ReadScoreboard(IDataReader reader) {
			ScoreboardStorage scoreboard = null;
			if(reader != null) {
				if(reader.Read()) {
					scoreboard = new ScoreboardStorage(
						reader.GetInt32(0),
						reader.GetInt32(1),
						reader.GetInt32(2),
						reader.GetInt32(3),
						reader.GetInt32(4)
					);
				}
				reader.Close();
			}
			return scoreboard;
		}
	}
}
