using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MTCG.Parse {
	class JsonSerializer {
		public static string Serialize(IEnumerable values) {
			string content = JsonConvert.SerializeObject(values, Formatting.Indented);
			return content;
		}
	}
}
