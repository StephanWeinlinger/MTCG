using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Battle.Card;
using MTCG.Database.Storage;
using MTCG.Database.Table;
using MTCG.Exception;
using MTCG.Http;
using MTCG.Http.ResponseContent;
using MTCG.Parse;

namespace MTCG.Controller {
	sealed class CardController : Controller {
		public CardController(HttpRequest request) : base(request) {
			Handle();
		}

		public override void Handle() {
			switch(Request.Method) {
				case HttpMethod.GET:
					CheckAuth();
					GetCards();
					break;
				default:
					ResponseContent = new ResponseNotFound();
					break;
			}
		}

		private void GetCards() {
			Database.Data = new Dictionary<string, string> {
				{ "owner", CurrentUserId }
			};
			var cards = CardTable.GetAllCardsFromUser(Database);
			if(cards == null || cards.Count == 0) {
				throw new BadRequestException("User doesn't own any cards");
			}
			ResponseContent = new ResponseOk(JsonSerializer.Serialize(cards), false);
		}

	}
}
