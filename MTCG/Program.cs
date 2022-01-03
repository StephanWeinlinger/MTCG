using System;
using System.Net;
using System.Net.Sockets;
using MTCG.Http;

namespace MTCG {
    class Program {
        static void Main(string[] args) {
			Console.CancelKeyPress += (sender, e) => Environment.Exit(0);
			new HttpServer(10001).Run();
        }
		// maybe wait for all tasks to finish
    }
}
