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
	sealed class BattleController : Controller {
		public BattleController(HttpRequest request) : base(request) {
			Handle();
		}

		public override void Handle() {
			switch(Request.Method) {
				case HttpMethod.POST:
					CheckAuth();
					StartBattle();
					break;
				default:
					ResponseContent = new ResponseNotFound();
					break;
			}
		}

		// TODO dont forget to look at battle table when trying to trade cards or change the deck
		// TODO deck cannot be changed if user is currently in a battle
		private void StartBattle() {
			bool start = false;
			BattleStorage currentBattle = null;
			if(!CurrentUser.IsDeckSet) {
				throw new BadRequestException("Deck is not set");
			}
			lock(Lock.Lock.BattleLocker) {
				Database.Data = new Dictionary<string, string>();
				var battles = BattleTable.GetAllBattles(Database);
				// check if user is already in queue or match
				foreach(var battle in battles) {
					if(battle.User1 == CurrentUser.Id || battle.User2 == CurrentUser.Id) {
						throw new BadRequestException("User already in queue or battle");
					}
				}
				// sort out battles where no second user is set
				var possibleBattles = new List<BattleStorage>(battles.FindAll(battle => battle.User2 == null));
				// if nothing matches then either insert or update
				if(possibleBattles.Count == 0) {
					Database.Data = new Dictionary<string, string> {
						{ "user1", CurrentUserId }
					};
					int id = BattleTable.InsertBattle(Database);
				} else {
					Database.Data = new Dictionary<string, string> {
						{ "user2", CurrentUserId },
						{ "id", possibleBattles[0].Id.ToString() }
					};
					int id = BattleTable.UpdateBattle(Database);
					start = true;
					currentBattle = new BattleStorage(possibleBattles[0].Id, possibleBattles[0].User1, CurrentUser.Id);
				}
			}
			if(start) {
				// battle logic
				// fetch current decks
				Database.Data = new Dictionary<string, string> {
					{ "owner", currentBattle.User1.ToString() }
				};
				var deck1 = CardTable.GetAllCardsInDeck(Database);
				Database.Data = new Dictionary<string, string> {
					{ "owner", currentBattle.User2.ToString() }
				};
				var deck2 = CardTable.GetAllCardsInDeck(Database);

				// use new class here
				// return log class, with winner and loser id

				//Database.Data = new Dictionary<string, string> {
				//	{ "userid", "1" }
				//};
				//ScoreboardStorage winner = ScoreboardTable.GetScoreboard(Database);
				//Database.Data = new Dictionary<string, string> {
				//	{ "userid", "2" }
				//};
				//ScoreboardStorage loser = ScoreboardTable.GetScoreboard(Database);
				//Database.Data = new Dictionary<string, string> {
				//	{ "userid", winner.UserId.ToString() },
				//	{ "elo", (winner.Elo + 3).ToString() },
				//	{ "wins", (winner.Wins + 1).ToString() },
				//	{ "losses", winner.Losses.ToString() },
				//	{ "draws", winner.Draws.ToString() },
				//};
				//ScoreboardTable.UpdateScoreboard(Database);
				//Database.Data = new Dictionary<string, string> {
				//	{ "userid", winner.UserId.ToString() },
				//	{ "elo", (winner.Elo - 5).ToString() },
				//	{ "wins", winner.Wins.ToString() },
				//	{ "losses", (winner.Losses + 1).ToString() },
				//	{ "draws", winner.Draws.ToString() },
				//};
				//ScoreboardTable.UpdateScoreboard(Database);

				// change elo and stats
				// delete battle in db
				Database.Data = new Dictionary<string, string> {
					{ "id", currentBattle.Id.ToString() }
				};
				BattleTable.DeleteBattle(Database);
				// maybe return winner?
				ResponseContent = new ResponseOk("Matched succesfully", true);
			} else {
				ResponseContent = new ResponseOk("Started matchmaking", true);
			}
		}
	}
}
