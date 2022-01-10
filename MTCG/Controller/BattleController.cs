using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Battle.Log;
using MTCG.Database.Storage;
using MTCG.Database.Table;
using MTCG.Exception;
using MTCG.Http;
using MTCG.Http.ResponseContent;
using MTCG.Parse;

namespace MTCG.Controller {
	public sealed class BattleController : Controller {
		public BattleController(HttpRequest request) : base(request) {
			Handle();
		}

		public override void Handle() {
			switch(Request.Method) {
				case HttpMethod.POST:
					CheckAuth();
					StartBattle();
					break;
				case HttpMethod.DELETE:
					CheckAuth();
					RemoveBattle();
					break;
				default:
					ResponseContent = new ResponseNotFound();
					break;
			}
		}

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

				Battle.Battle battle = new Battle.Battle(deck1, deck2);
				battle.StartBattle();

				Log battlelog = battle.Log;

				// change elo and stats
				Database.Data = new Dictionary<string, string> {
					{ "userid", battlelog.Id1.ToString() }
				};
				ScoreboardStorage user1 = ScoreboardTable.GetScoreboard(Database);
				Database.Data = new Dictionary<string, string> {
					{ "userid", battlelog.Id2.ToString() }
				};
				ScoreboardStorage user2 = ScoreboardTable.GetScoreboard(Database);
				if(battlelog.IsDraw) {
					Database.Data = new Dictionary<string, string> {
						{ "userid", user1.UserId.ToString() },
						{ "elo", user1.Elo.ToString() },
						{ "wins", user1.Wins.ToString() },
						{ "losses", user1.Losses.ToString() },
						{ "draws", (user1.Draws + 1).ToString() },
					};
					ScoreboardTable.UpdateScoreboard(Database);
					Database.Data = new Dictionary<string, string> {
						{ "userid", user2.UserId.ToString() },
						{ "elo", user2.Elo.ToString() },
						{ "wins", user2.Wins.ToString() },
						{ "losses", user2.Losses.ToString() },
						{ "draws", (user2.Draws + 1).ToString() },
					};
					ScoreboardTable.UpdateScoreboard(Database);
				} else {
					var winner = user1.UserId == battlelog.WinnerId ? user1 : user2;
					var loser = user1.UserId == battlelog.LoserId ? user1 : user2;
					Database.Data = new Dictionary<string, string> {
						{ "userid", winner.UserId.ToString() },
						{ "elo", (winner.Elo + 3).ToString() },
						{ "wins", (winner.Wins + 1).ToString() },
						{ "losses", winner.Losses.ToString() },
						{ "draws", winner.Draws.ToString() },
					};
					ScoreboardTable.UpdateScoreboard(Database);
					Database.Data = new Dictionary<string, string> {
						{ "userid", loser.UserId.ToString() },
						{ "elo", (loser.Elo - 5).ToString() },
						{ "wins", loser.Wins.ToString() },
						{ "losses", (loser.Losses + 1).ToString() },
						{ "draws", loser.Draws.ToString() },
					};
					ScoreboardTable.UpdateScoreboard(Database);
				}
				
				// delete battle in db
				Database.Data = new Dictionary<string, string> {
					{ "id", currentBattle.Id.ToString() }
				};
				BattleTable.DeleteBattle(Database);
				ResponseContent = new ResponseOk(JsonSerializer.Serialize(battlelog), false);
			} else {
				ResponseContent = new ResponseOk("Started matchmaking", true);
			}
		}

		private void RemoveBattle() {
			Database.Data = new Dictionary<string, string> {
					{ "user1", CurrentUserId }
			};

			if(BattleTable.DeleteBattleByUser1(Database)) {
				throw new BadRequestException("User is currently not matchmaking");
			}
			ResponseContent = new ResponseOk("User was removed from matchmaking", true);
		}
	}
}
