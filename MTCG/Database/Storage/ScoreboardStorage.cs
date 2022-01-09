using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Database.Storage {
	class ScoreboardStorage : IStorage {
		public ScoreboardStorage(int userId, int elo, int wins, int losses, int draws) {
			UserId = userId;
			Elo = elo;
			Wins = wins;
			Losses = losses;
			Draws = draws;
		}
		public int UserId;
		public int Elo;
		public int Wins;
		public int Losses;
		public int Draws;
	}
}
