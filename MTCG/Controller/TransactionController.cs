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
	public sealed class TransactionController : Controller {
		public TransactionController(HttpRequest request) : base(request) {
			Handle();
		}

		public override void Handle() {
			switch(Request.Method) {
				case HttpMethod.POST:
					CheckAuth();
					BuyPackage();
					break;
				default:
					ResponseContent = new ResponseNotFound();
					break;
			}
		}

		private void BuyPackage() {
			// update user if there are atleast 5 coins in the wallet
			Database.Data = new Dictionary<string, string> {
				{ "id", CurrentUserId }
			};
			int id = UserTable.UpdateCoinsOnBuy(Database);
			if(id == -1) {
				throw new BadRequestException("Not enough money to buy package");
			}
			// get first package and delete it
			PackageStorage package = null;
			lock(Lock.Lock.PackageLocker) {
				Database.Data = new Dictionary<string, string>();
				package = PackageTable.GetFirstPackage(Database);
				if(package != null) {
					// delete it
					Database.Data = new Dictionary<string, string> {
						{ "id", package.Id.ToString()}
					};
					PackageTable.DeletePackage(Database);
				}
			}
			if(package == null) {
				// refund money
				Database.Data = new Dictionary<string, string> {
					{ "id", CurrentUserId }
				};
				UserTable.UpdateCoinsOnRefund(Database);
				throw new BadRequestException("Packages currently not available");
			}
			// change card owner
			var cards = new List<int> {
				package.Card1,
				package.Card2,
				package.Card3,
				package.Card4,
				package.Card5
			};
			foreach(var entry in cards) {
				Database.Data = new Dictionary<string, string> {
					{ "owner", CurrentUserId},
					{ "id", entry.ToString()}
				};
				CardTable.UpdateCard(Database);
			}
			ResponseContent = new ResponseOk("Package was bought", true);
		}
	}
}
