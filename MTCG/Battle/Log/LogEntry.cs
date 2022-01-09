using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Battle.Card;

namespace MTCG.Battle.Log {
	class LogEntry {
		private int _actWinnerDamage;
		private int _actLoserDamage;
		private int _baseWinnerDamage;
		private int _baseLoserDamage;
		private string _winnerCard;
		private string _loserCard;
		private string _message;

		public LogEntry(ICard winner, ICard loser, int winnerDamage, int loserDamage, bool isDraw) {
			_actWinnerDamage = winnerDamage;
			_actLoserDamage = loserDamage;
			_baseWinnerDamage = winner.Damage;
			_baseLoserDamage = loser.Damage;
			_winnerCard = $"{winner.Name.ToString()} {winner.Type.ToString()} {winner.Element.ToString()}"; // Name, Type, Element
			_loserCard = $"{loser.Name.ToString()} {loser.Type.ToString()} {loser.Element.ToString()}"; // Name, Type, Element
			_message = isDraw ? $"{winner.Name.ToString()} tied against {loser.Name.ToString()}" : $"{winner.Name.ToString()} won against {loser.Name.ToString()}";
			Console.WriteLine(_message);
		}
	}
}
