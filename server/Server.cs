using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class Server
    {
        private int port;
        private const int MAX_PLAYERS_IN_THE_GAME = 4;
        private Arena arena;
        private PlayerConnection[] players = new PlayerConnection[MAX_PLAYERS_IN_THE_GAME];
        private Socket listener;

        public Server(int port)
        {
            CreateSocket();
            this.port = port;
        }


        public void Start()
        {
            Console.WriteLine("Starting server");
            AcceptPlayers().ContinueWith((t) => Console.WriteLine("Task has continued"));
        }

        private async Task AcceptPlayers()
        {
            Socket handler = await listener.AcceptAsync();
            Console.WriteLine("Start to accept");

            var bytes = new byte[1024];

            // Receive the response from the remote device.  
            var bytesRec = handler.Receive(bytes);
            var playerName = Encoding.UTF8.GetString(bytes, 0, bytesRec);

            Console.WriteLine("Nome jogador: {0}", playerName);

            if (players.Length == MAX_PLAYERS_IN_THE_GAME)
            {
                handler.Send(Encoding.UTF8.GetBytes("The game is already full"));
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            else
            {
                players[players.Length] = new PlayerConnection(playerName, handler);
            }
        }

        private void CreateSocket()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Socket connected to {0}", localEndPoint.ToString());
        }

        class PlayerConnection
        {
            public Player Player { get; }
            private Socket socket;

            public PlayerConnection(string playerName, Socket socket)
            {
                this.Player = new Player(playerName);
                this.socket = socket;
            }
        }
    }
}
