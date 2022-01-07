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
	sealed class PackageController : Controller {
		public PackageController(HttpRequest request) : base(request) {
			Handle();
		}

		public override void Handle() {
			switch(Request.Method) {
				case HttpMethod.POST:
					CheckAuth();
					InsertPackage();
					break;
				default:
					ResponseContent = new ResponseNotFound();
					break;
			}
		}

		private void InsertPackage() {
			int packageId = -1;
			if(CurrentUser.Username == "admin") {
				var data = JsonDeserializer.Deserialize<List<Dictionary<string, string>>>(Request.Content, DeserializeType.CREATE_PACKAGE);
				var packageData = new Dictionary<string, string>();
				for(int i = 0; i < 5; ++i) {
					Database.Data = data[i];
					int cardId = CardTable.InsertCard(Database);
					if(cardId == -1) {
						throw new BadRequestException("Card could not be inserted");
					}
					packageData.Add($"card{i + 1}", cardId.ToString());
				}
				Database.Data = packageData;
				packageId = PackageTable.InsertPackage(Database);
			}
			if(packageId == -1) {
				throw new BadRequestException("Package could not be inserted");
			}
			ResponseContent = new ResponseOk("Package was inserted", true);
		}
	}
}
