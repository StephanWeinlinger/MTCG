using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Http.ResponseContent {
	class ResponseContent {
		public string Content { get; private set; }
		public int Code { get; private set; }
		public string Message { get; private set; }
		protected bool Error;

		protected ResponseContent(int code, string message) {
			Code = code;
			Message = message;
			Content = "";
		}

		protected ResponseContent(int code, string message, string content, bool isMessage) {
			Code = code;
			Message = message;
			Content = "";
		}

		public void SetContent(string content, bool isMessage) {
			Console.WriteLine(content);
			// sets simple messages
			if(isMessage) {
				Content = $"{{\"error\": {Error.ToString().ToLower()}, \"message\": \"{content}\"}}";
				return;
			}
			// sets objects
			if(!Error) {
				Content = content;
			}
		}
	}
}
