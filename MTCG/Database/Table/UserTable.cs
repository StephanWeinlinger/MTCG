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

		public static IStorage GetUserByToken(Database db, IDictionary<string, string> data) {
			string statement = "SELECT * FROM \"user\" WHERE token = @token";
			var fields = new Dictionary<string, NpgsqlDbType> {
				{ "token", NpgsqlDbType.Varchar },
			};
			db.PrepareCommand(statement, fields);
			IStorage user = ReadUser(db.ExecuteCommandWithRead(data));
			return user;
		}

		public static int InsertUser(Database db, IDictionary<string, string> data) {
			string statement = "INSERT INTO \"user\" (username, password, token) VALUES (@username, @password, @token) RETURNING id";
			var fields = new Dictionary<string, NpgsqlDbType> {
				{ "username", NpgsqlDbType.Varchar },
				{ "password", NpgsqlDbType.Varchar },
				{ "token", NpgsqlDbType.Varchar }
			};
			db.PrepareCommand(statement, fields);
			int id = ReadId(db.ExecuteCommandWithRead(data));
			return id;
		}

		public static int UpdateUser(Database db, IDictionary<string, string> data) {
			string statement = "UPDATE \"user\" SET displayname = @displayname, bio = @bio, status = @status WHERE token = @token RETURNING id";
			var fields = new Dictionary<string, NpgsqlDbType> {
				{ "displayname", NpgsqlDbType.Varchar },
				{ "bio", NpgsqlDbType.Varchar },
				{ "status", NpgsqlDbType.Varchar },
				{ "token", NpgsqlDbType.Varchar }
			};
			db.PrepareCommand(statement, fields);
			int id = ReadId(db.ExecuteCommandWithRead(data));
			return id;
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

		private static IStorage ReadUser(IDataReader reader) {
			IStorage user;
			if(reader != null && reader.Read()) {
				user = new UserStorage(
					reader.GetInt32(0),
					reader.GetString(1),
					reader.GetString(2),
					reader.GetString(3),
					reader.GetBoolean(4),
					reader.GetInt32(5),
					reader.GetString(6),
					reader.GetString(7),
					reader.GetString(8)
				);
			} else {
				user = null;
			}
			return user;
		}
	}
}
