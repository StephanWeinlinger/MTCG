using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Database.Storage;
using NpgsqlTypes;

namespace MTCG.Database.Table {
	public static class PackageTable {
		public static PackageStorage GetFirstPackage(Database db) {
			db.Statement = "SELECT * FROM \"package\" LIMIT 1";
			db.Fields = new Dictionary<string, NpgsqlDbType>();
			db.PrepareCommand();
			PackageStorage package = ReadPackage(db.ExecuteCommandWithRead());
			return package;
		}

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

		public static bool DeletePackage(Database db) {
			db.Statement = "DELETE FROM \"package\" WHERE id = @id";
			db.Fields = new Dictionary<string, NpgsqlDbType> {
				{ "id", NpgsqlDbType.Integer }
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

		private static PackageStorage ReadPackage(IDataReader reader) {
			PackageStorage package = null;
			if(reader != null) {
				if(reader.Read()) {
					package = new PackageStorage(
						reader.GetInt32(0),
						reader.GetInt32(1),
						reader.GetInt32(2),
						reader.GetInt32(3),
						reader.GetInt32(4),
						reader.GetInt32(5)
					);
				}
				reader.Close();
			}
			return package;
		}
	}
}
