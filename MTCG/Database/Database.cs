using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace MTCG.Database {
	class Database {
		private IDbConnection _connection;
		private IDbCommand _command;
		public Database() {
			_connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=root;Database=MTCG");
			_connection.Open();
			_command = _connection.CreateCommand();
		}

		~Database() {
			_connection.Close();
		}

		// TODO allow length for fields
		public void PrepareCommand(string statement, Dictionary<string, NpgsqlDbType> fields) {
			_command.CommandText = statement;
			NpgsqlCommand c = _command as NpgsqlCommand;
			foreach(KeyValuePair<string, NpgsqlDbType> entry in fields) {
				if(entry.Value == NpgsqlDbType.Varchar) {
					c.Parameters.Add(entry.Key, entry.Value, 50);
				} else {
					c.Parameters.Add(entry.Key, entry.Value);
				}
			}
			c.Prepare();
		}

		public void ExecuteCommandWithoutRead(Dictionary<string, string> data) {
			NpgsqlCommand c = _command as NpgsqlCommand;
			foreach(KeyValuePair<string, string> entry in data) {
				c.Parameters[entry.Key].Value = entry.Value;
			}

			_command.ExecuteNonQuery();
		}

		public IDataReader ExecuteCommandWithRead(Dictionary<string, string> data) {
			NpgsqlCommand c = _command as NpgsqlCommand;
			foreach(KeyValuePair<string, string> entry in data) {
				c.Parameters[entry.Key].Value = entry.Value;
			}

			IDataReader reader = _command.ExecuteReader();
			return reader;
			// TODO you have to remember to close the reader
		}
	}
}
