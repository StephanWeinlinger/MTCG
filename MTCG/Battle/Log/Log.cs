using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Battle.Card;

namespace MTCG.Battle.Log {
	class Log {
		public int WinnerId;
		public int LoserId;
		public bool IsDraw;
		public IList<LogEntry> Rounds;

		public Log() {
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
