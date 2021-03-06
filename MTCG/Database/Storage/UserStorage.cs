using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Database.Storage {
	public class UserStorage : IStorage {
		public UserStorage(int id, string username, string password, string token, bool isLoggedIn, int coins, string displayname, string bio, string status, bool isDeckSet) {
			Id = id;
			Username = username;
			Password = password;
			Token = token;
			IsLoggedIn = isLoggedIn;
			Coins = coins;
			Displayname = displayname;
			Bio = bio;
			Status = status;
			IsDeckSet = isDeckSet;
		}
		public int Id;
		public string Username;
		public string Password;
		public string Token;
		public bool IsLoggedIn;
		public int Coins;
		public string Displayname;
		public string Bio;
		public string Status;
		public bool IsDeckSet;
	}
}
