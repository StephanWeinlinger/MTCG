﻿using System;
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

		private static UserStorage ReadUser(IDataReader reader) {
			UserStorage user;
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
				reader.Close();
			} else {
				user = null;
			}
			return user;
		}
	}
}
