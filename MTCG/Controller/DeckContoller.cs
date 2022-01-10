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
	sealed class Deckcontroller : Controller {
		public Deckcontroller(HttpRequest request) : base(request) {
			Handle();
		}

		public override void Handle() {
			switch(Request.Method) {
				case HttpMethod.GET:
					CheckAuth();
					GetDeck();
					break;
				case HttpMethod.PUT:
					CheckAuth();
					UpdateDeck();
					break;
				default:
					ResponseContent = new ResponseNotFound();
					break;
			}
		}

		private void GetDeck() {
			if(!CurrentUser.IsDeckSet) {
				throw new BadRequestException("Deck is not set");
			}
			Database.Data = new Dictionary<string, string> {
				{ "owner", CurrentUserId }
			};
			var cards = CardTable.GetAllCardsInDeckJoined(Database);
			if(cards == null || cards.Count == 0) {
				throw new BadRequestException("Deck is not set");
			}
			ResponseContent = new ResponseOk(JsonSerializer.Serialize(cards), false);
		}

		private void UpdateDeck() {
			// check if the cards are even owned by the user (done first because it can be done without locking the battle)
			lock(Lock.Lock.TradeInsertLocker) {
				var data = JsonDeserializer.Deserialize<List<string>>(Request.Content, DeserializeType.CONFIGURE_DECK);
				Database.Data = new Dictionary<string, string> {
					{ "owner", CurrentUserId }
				};
				var cards = CardTable.GetAllCardsFromUserJoined(Database);
				foreach(var entry in data) {
					if(!cards.Exists(card => card.Id == int.Parse(entry))) {
						throw new BadRequestException("Used card is not owned by user");
					}
				}
				lock(Lock.Lock.BattleLocker) {
					// get all battles
					Database.Data = new Dictionary<string, string>();
					var battles = BattleTable.GetAllBattles(Database);
					foreach(var battle in battles) {
						if(battle.User1 == CurrentUser.Id || battle.User2 == CurrentUser.Id) {
							throw new BadRequestException("User currently in queue or battle");
						}
					}

					if(CurrentUser.IsDeckSet) {
						// update old cards
						Database.Data = new Dictionary<string, string> {
							{ "owner", CurrentUserId }
						};
						CardTable.UpdateAllCardsOldDeck(Database);
					}
					// update new cards
					Database.Data = new Dictionary<string, string> {
						{ "id", "" },
					};
					foreach(var entry in data) {
						Database.Data["id"] = entry;
						CardTable.UpdateCardNewDeck(Database);
					}
					// update isdeckset
					Database.Data = new Dictionary<string, string> {
						{ "isdeckset", "true"},
						{ "id", CurrentUserId }
					};
					UserTable.UpdateIsDeckSet(Database);
				}
			}
			ResponseContent = new ResponseOk("Deck was updated", true);
		}
	}
}
