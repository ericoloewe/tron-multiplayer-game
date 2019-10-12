using System;

namespace server
{
    class Program
    {
        private static Server server = new Server(8080);

        static void Main(string[] args) => server.Start().Wait();
    }
}
