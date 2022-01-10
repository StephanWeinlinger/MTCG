using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Battle.Card;

namespace MTCG.Battle.Log {
	public class LogEntry {
		public int ActWinnerDamage;
		public int ActLoserDamage;
		public int BaseWinnerDamage;
		public int BaseLoserDamage;
		public string WinnerCard;
		public string LoserCard;
		public string Message;

		public LogEntry(ICard winner, ICard loser, int winnerDamage, int loserDamage, bool isDraw) {
			ActWinnerDamage = winnerDamage;
			ActLoserDamage = loserDamage;
			BaseWinnerDamage = winner.Damage;
			BaseLoserDamage = loser.Damage;
			WinnerCard = $"{winner.Type.ToString()} | {winner.Name.ToString()} | {winner.Element.ToString()}"; // Name, Type, Element
			LoserCard = $"{loser.Type.ToString()} | {loser.Name.ToString()} | {loser.Element.ToString()}"; // Name, Type, Element
			Message = isDraw ? $"{winner.Name.ToString()} tied against {loser.Name.ToString()}" : $"{winner.Name.ToString()} won against {loser.Name.ToString()}";
		}
	}
}
