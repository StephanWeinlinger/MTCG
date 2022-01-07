using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Exception;
using Newtonsoft.Json;

// TODO check required fields if they actuallly contain something
namespace MTCG.Parse {
	static class JsonDeserializer {
		public static T Deserialize<T>(string content, DeserializeType type) {
			var neededEntries = new List<string>();
			var neededAmount = 0;
			switch(type) {
				case DeserializeType.EDIT_USER:
					neededEntries.AddRange(new List<string> { "displayname", "bio", "status" });
					break;
				case DeserializeType.CREATE_TRADE:
					neededEntries.AddRange(new List<string> { "cardID", "wantedType", "wantedElement", "minDamage" });
					break;
				case DeserializeType.DO_TRADE:
					neededEntries.AddRange(new List<string> { "id", "cardID" });
					break;
				case DeserializeType.CONFIGURE_DECK:
					neededAmount = 4;
					break;
				case DeserializeType.CREATE_PACKAGE:
					neededAmount = 5;
					neededEntries.AddRange(new List<string> { "name", "damage" });
					break;
				case DeserializeType.REGISTER_USER:
				case DeserializeType.LOGIN_USER:
					neededEntries.AddRange(new List<string> { "username", "password" });
					break;
			}
			IEnumerable values = null;
			var contentType = typeof(T);
			if(contentType == typeof(List<string>)) {
				values = JsonConvert.DeserializeObject<List<string>>(content);
				CheckAmount((IList<string>) values, neededAmount);
			} else if(contentType == typeof(Dictionary<string, string>)) {
				values = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
				CheckContents((IDictionary<string, string>) values, neededEntries);
			} else if(contentType == typeof(List<Dictionary<string, string>>)) {
				values = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(content);
				CheckAmount((IList<Dictionary<string, string>>) values, neededAmount);
				foreach(Dictionary<string, string> entry in values) {
					CheckContents(entry, neededEntries);
				}
			}
			return (T) Convert.ChangeType(values, typeof(T));
		}

		private static void CheckContents(IDictionary<string, string> values, IList<string> neededEntries) {
			bool error = false;
			foreach(string entry in neededEntries) {
				if(!values.ContainsKey(entry)) {
					error = true;
					break;
				}
			}
			if(values.Count != neededEntries.Count) {
				error = true;
			}
			if(error) {
				throw new BadRequestException("Body contents don't match with requirements");
			}
		}

		private static void CheckAmount<T>(IList<T> values, int amount) {
			if(values.Count != amount) {
				throw new BadRequestException("Body contents don't match with requirements");
			}
		}
	}
}