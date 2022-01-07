using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Database.Storage;
using NpgsqlTypes;

namespace MTCG.Database.Table {
	static class PackageTable {

		public static int InsertPackage(Database db) {
			db.Statement = "INSERT INTO \"package\" (card1, card2, card3, card4, card5) VALUES (@card1, @card2, @card3, @card4, @card5) RETURNING id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "card1", NpgsqlDbType.Integer },
				{ "card2", NpgsqlDbType.Integer },
				{ "card3", NpgsqlDbType.Integer },
				{ "card4", NpgsqlDbType.Integer },
				{ "card5", NpgsqlDbType.Integer },
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
