using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Database.Storage;
using MTCG.Database.Table;
using MTCG.Exception;
using MTCG.Http;
using MTCG.Http.ResponseContent;
using MTCG.Parse;

namespace MTCG.Controller {
	sealed class LeaderboardController : Controller {
		public LeaderboardController(HttpRequest request) : base(request) {
			Handle();
		}

		public override void Handle() {
			switch(Request.Method) {
				case HttpMethod.GET:
					CheckAuth();
					GetLeaderboard();
					break;
				default:
					ResponseContent = new ResponseNotFound();
					break;
			}
		}

		private void GetLeaderboard() {
			Database.Data = new Dictionary<string, string>();
			var leaderboard = ScoreboardTable.GetAllScoreboardsOrdered(Database);
			if(leaderboard == null || leaderboard.Count == 0) {
				throw new BadRequestException("Leaderboard is currently not available");
			}
			ResponseContent = new ResponseOk(JsonSerializer.Serialize(leaderboard), false);
		}
	}
}
