using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Battle.Card;

namespace MTCG.Battle.Log {
	class Log {
		public int Id1;
		public int Id2;
		public int WinnerId;
		public int LoserId;
		public bool IsDraw;
		public IList<LogEntry> Rounds;

		public Log(int id1, int id2) {
			Id1 = id1;
			Id2 = id2;
			WinnerId = -1;
			LoserId = -1;
			IsDraw = false;
			Rounds = new List<LogEntry>();
		}

		public void AddEntry(ICard winner, ICard loser, int winnerDamage, int loserDamage, bool isDraw) {
			Rounds.Add(new LogEntry(winner, loser, winnerDamage, loserDamage, isDraw));
		}
	}
}
