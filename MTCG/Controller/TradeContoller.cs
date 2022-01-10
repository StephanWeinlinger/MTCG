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
using Type = MTCG.Battle.Card.Type;

namespace MTCG.Controller {
	sealed class TradeController : Controller {
		public TradeController(HttpRequest request) : base(request) {
			Handle();
		}

		public override void Handle() {
			switch(Request.Method) {
				case HttpMethod.GET:
					CheckAuth();
					GetTrades();
					break;
				case HttpMethod.POST:
					CheckAuth();
					if(Request.PathContents.Count == 2) {
						DoTrade();
					} else {
						InsertTrade();
					}
					break;
				case HttpMethod.DELETE:
					CheckAuth();
					RemoveTrade();
					break;
				default:
					ResponseContent = new ResponseNotFound();
					break;
			}
		}

		private void GetTrades() {
			Database.Data = new Dictionary<string, string>();
			var trades = TradeTable.GetAllTradesJoined(Database);
			if(trades == null || trades.Count == 0) {
				throw new BadRequestException("Trades currently not available");
			}
			ResponseContent = new ResponseOk(JsonSerializer.Serialize(trades), false);
		}

		private void InsertTrade() {
			var data = JsonDeserializer.Deserialize<Dictionary<string, string>>(Request.Content, DeserializeType.CREATE_TRADE);
			// change strings to numbers
			if(Enum.TryParse(data["wantedtype"], out Type tmpWantedType)) {
				data["wantedtype"] = ((int) tmpWantedType).ToString();
			} else {
				data["wantedtype"] = null;
			}
			if(Enum.TryParse(data["wantedelement"], out Element tmpWantedElement)) {
				data["wantedelement"] = ((int) tmpWantedElement).ToString();
			} else {
				data["wantedelement"] = null;
			}
			if(data["wantedtype"] == null && data["wantedelement"] == null) {
				throw new BadRequestException("wantedtype and / or wantedelement are invalid");
			}

			lock(Lock.Lock.TradeInsertLocker) {
				// check if user owns the card and doesn't use it in the deck
				Database.Data = new Dictionary<string, string> {
					{ "owner", CurrentUserId }
				};
				var cards = CardTable.GetAllCardsFromUserJoined(Database);
				if(!cards.Exists(card => card.Id.ToString() == data["cardid"] && !card.InDeck)) {
					throw new BadRequestException("Card can't be traded");
				}
				// remove card from owner
				Database.Data = new Dictionary<string, string> {
					{ "owner", null },
					{ "id", data["cardid"]}
				};
				CardTable.UpdateCard(Database);
			}
			// insert trade
			Database.Data = data;
			Database.Data.Add("seller", CurrentUserId);
			int id = TradeTable.InsertTrade(Database);
			if(id == -1) {
				// give card back to user
				Database.Data = new Dictionary<string, string> {
					{ "owner", CurrentUserId },
					{ "id", data["cardid"]}
				};
				CardTable.UpdateCard(Database);
				throw new BadRequestException("Trade creation failed");
			}
			ResponseContent = new ResponseOk("Trade was created", true);
		}

		private void DoTrade() {
			// get card
			Database.Data = JsonDeserializer.Deserialize<Dictionary<string, string>>(Request.Content, DeserializeType.DO_TRADE);
			var card = CardTable.GetCard(Database);
			if(card == null || card.Owner != CurrentUser.Id) {
				throw new BadRequestException("Card to trade doesn't exist or is owned by someone else");
			}
			// remove card from owner early, in order to decrease lock time
			lock(Lock.Lock.TradeInsertLocker) {
				Database.Data = new Dictionary<string, string> {
					{ "owner", null },
					{ "id", card.Id.ToString() }
				};
				CardTable.UpdateCard(Database);
			}

			TradeStorage trade = null;
			lock(Lock.Lock.TradeDoLocker) {
				Database.Data = new Dictionary<string, string> {
					{ "id", Request.PathContents[1] }
				};
				trade = TradeTable.GetTrade(Database);
				string message = null;
				if(trade == null) {
					message = "Trade doesn't exist";
				} else if(card.Damage < trade.MinDamage) {
					message = "Damage is not enough";
				} else if(trade.WantedType != null && card.Type != trade.WantedType) {
					message = "Type doesn't match";
				} else if(trade.WantedElement != null && card.Element != trade.WantedElement) {
					message = "Element doesn't match";
				} else if(trade.Seller == CurrentUser.Id) {
					message = "User can't buy own card";
				}
				if(message != null) {
					throw new BadRequestException(message);
				}
				// remove trade
				Database.Data = new Dictionary<string, string> {
					{"id", trade.Id.ToString()}
				};
				if(TradeTable.DeleteTrade(Database)) {
					throw new BadRequestException("Trade could not be completed");
				}
			}
			// give card to buyer
			Database.Data = new Dictionary<string, string> {
				{"owner", CurrentUserId},
				{"id", trade.CardId.ToString()}
			};
			CardTable.UpdateCard(Database);
			// give card to creator
			Database.Data = new Dictionary<string, string> {
				{"owner", trade.Seller.ToString()},
				{"id", card.Id.ToString()}
			};
			CardTable.UpdateCard(Database);
			ResponseContent = new ResponseOk("Trade was completed", true);
		}

		private void RemoveTrade() {
			if(Request.PathContents.Count != 2) {
				throw new BadRequestException("Trade not found");
			}

			TradeStorage trade = null;
			lock(Lock.Lock.TradeDoLocker) {
				// get trade
				Database.Data = new Dictionary<string, string> {
					{ "id", Request.PathContents[1] }
				};
				trade = TradeTable.GetTrade(Database);
				if(trade == null || trade.Seller != CurrentUser.Id) {
					throw new BadRequestException("Trade doesn't exist or was initiated by someone else");
				}
				// delete trade
				if(TradeTable.DeleteTrade(Database)) {
					throw new BadRequestException("Trade could not be deleted");
				}
			}
			// give card back to seller
			Database.Data = new Dictionary<string, string> {
				{ "owner", trade.Seller.ToString() },
				{ "id", trade.CardId.ToString() }
			};
			CardTable.UpdateCard(Database);

			ResponseContent = new ResponseOk("Trade was deleted", true);
		}
	}
}
