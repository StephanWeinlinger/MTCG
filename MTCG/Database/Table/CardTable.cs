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
	}
}
