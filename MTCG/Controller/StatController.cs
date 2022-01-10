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
	public sealed class StatController : Controller {
		public StatController(HttpRequest request) : base(request) {
			Handle();
		}

		public override void Handle() {
			switch(Request.Method) {
				case HttpMethod.GET:
					CheckAuth();
					GetStats();
					break;
				default:
					ResponseContent = new ResponseNotFound();
					break;
			}
		}

		private void GetStats() {
			Database.Data = new Dictionary<string, string> {
				{ "userid", CurrentUserId }
			};
			var scoreboard = ScoreboardTable.GetScoreboard(Database);
			if(scoreboard == null) {
				throw new BadRequestException("Stats could not be found");
			}
			ResponseContent = new ResponseOk(JsonSerializer.Serialize(scoreboard), false);
		}
	}
}
