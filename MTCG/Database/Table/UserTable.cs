using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Database.Storage;
using NpgsqlTypes;

namespace MTCG.Database.Table {
	static class UserTable {

		public static UserStorage GetUserByToken(Database db) {
			db.Statement = "SELECT * FROM \"user\" WHERE token = @token";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "token", NpgsqlDbType.Varchar },
			};
			db.PrepareCommand();
			UserStorage user = ReadUser(db.ExecuteCommandWithRead());
			return user;
		}

		public static UserStorage GetUserByUsername(Database db) {
			db.Statement = "SELECT * FROM \"user\" WHERE username = @username";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "username", NpgsqlDbType.Varchar },
			};
			db.PrepareCommand();
			UserStorage user = ReadUser(db.ExecuteCommandWithRead());
			return user;
		}

		public static int InsertUser(Database db) {
			db.Statement = "INSERT INTO \"user\" (username, password, token) VALUES (@username, @password, @token) RETURNING id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "username", NpgsqlDbType.Varchar },
				{ "password", NpgsqlDbType.Varchar },
				{ "token", NpgsqlDbType.Varchar }
			};
			db.PrepareCommand();
			int id = ReadId(db.ExecuteCommandWithRead());
			return id;
		}

		public static int UpdateUser(Database db) {
			db.Statement = "UPDATE \"user\" SET displayname = @displayname, bio = @bio, status = @status WHERE id = @id RETURNING id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "displayname", NpgsqlDbType.Varchar },
				{ "bio", NpgsqlDbType.Varchar },
				{ "status", NpgsqlDbType.Varchar },
				{ "id", NpgsqlDbType.Integer }
			};
			db.PrepareCommand();
			int id = ReadId(db.ExecuteCommandWithRead());
			return id;
		}

		public static int UpdateIsLoggedIn(Database db) {
			db.Statement = "UPDATE \"user\" SET isloggedin = @isloggedin WHERE id = @id RETURNING id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "isloggedin", NpgsqlDbType.Boolean },
				{ "id", NpgsqlDbType.Integer }
			};
			db.PrepareCommand();
			int id = ReadId(db.ExecuteCommandWithRead());
			return id;
		}

		public static int UpdateCoinsOnBuy(Database db) {
			db.Statement = "UPDATE \"user\" SET coins = ( SELECT coins FROM \"user\" WHERE id = @id ) - 5 WHERE id = ( SELECT id FROM \"user\" WHERE id = @id AND coins > 4) RETURNING id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "id", NpgsqlDbType.Integer }
			};
			db.PrepareCommand();
			int id = ReadId(db.ExecuteCommandWithRead());
			return id;
		}

		public static int UpdateCoinsOnRefund(Database db) {
			db.Statement = "UPDATE \"user\" SET coins = ( SELECT coins FROM \"user\" WHERE id = @id ) + 5 WHERE id = @id RETURNING id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "id", NpgsqlDbType.Integer }
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

		private static UserStorage ReadUser(IDataReader reader) {
			UserStorage user = null;
			if(reader != null) {
				if(reader.Read()) {
					string displayname = !reader.IsDBNull(6) ? reader.GetString(6) : null;
					string bio = !reader.IsDBNull(7) ? reader.GetString(7) : null;
					string status = !reader.IsDBNull(8) ? reader.GetString(8) : null;
					user = new UserStorage(
						reader.GetInt32(0),
						reader.GetString(1),
						reader.GetString(2),
						reader.GetString(3),
						reader.GetBoolean(4),
						reader.GetInt32(5),
						displayname,
						bio,
						status,
						reader.GetBoolean(9)
					);

				}
				reader.Close();
			}
			return user;
		}
	}
}
