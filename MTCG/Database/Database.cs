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
		public string Statement;
		public IDictionary<string, NpgsqlDbType> Fields;
		public IDictionary<string, string> Data;
		public Database() {
			_connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=root;Database=mtcg");
			_connection.Open();
		}

		~Database() {
			_connection.Close();
		}

		// TODO allow length for fields
		public void PrepareCommand() {
			_command = _connection.CreateCommand();
			_command.CommandText = Statement;
			NpgsqlCommand c = _command as NpgsqlCommand;
			foreach(KeyValuePair<string, NpgsqlDbType> entry in Fields) {
				if(entry.Value == NpgsqlDbType.Varchar) {
					c.Parameters.Add(entry.Key, entry.Value, 50);
				} else {
					c.Parameters.Add(entry.Key, entry.Value);
				}
			}
			c.Prepare();
		}

		// TODO maybe also use amount of executing to use prepared?
		public bool ExecuteCommandWithoutRead() {
			try {
				NpgsqlCommand c = _command as NpgsqlCommand;
				foreach(KeyValuePair<string, string> entry in Data) {
					c.Parameters[entry.Key].Value = entry.Value;
				}
				_command.ExecuteNonQuery();
			} catch(System.Exception e) {
				Console.WriteLine("exception in database");
				Console.WriteLine(e.Message);
				return false;
			}
			return true;
		}
		// TODO null values dont get inserted
		public IDataReader ExecuteCommandWithRead() {
			try {
				NpgsqlCommand c = _command as NpgsqlCommand;
				foreach(KeyValuePair<string, string> entry in Data) {
						switch (Fields[entry.Key]) {
							case NpgsqlDbType.Varchar:
								c.Parameters[entry.Key].Value = entry.Value;
								break;
							case NpgsqlDbType.Integer:
								c.Parameters[entry.Key].Value = int.Parse(entry.Value);
								break;
							case NpgsqlDbType.Boolean:
								c.Parameters[entry.Key].Value = bool.Parse(entry.Value);
								break;
							default:
								throw new TypeAccessException("Unhandled DB type");
					}
				}
				IDataReader reader = _command.ExecuteReader();
				return reader;
			} catch(System.Exception e) {
				Console.WriteLine("exception in database");
				Console.WriteLine(e.Message);
			}
			return null;
			// TODO you have to remember to close the reader
		}
	}
}
