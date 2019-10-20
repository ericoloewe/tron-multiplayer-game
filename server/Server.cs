using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    partial class Server
    {
        private Arena arena;
        private IList<PlayerConnection> players = new List<PlayerConnection>();
        private int port;
        private Socket listener;

        public Server(int port)
        {
            this.arena = new Arena();
            this.port = port;
            CreateSocket();
        }


        public async Task Start()
        {
            Console.WriteLine("Starting server");
            await StartCycle();
        }

        private async Task StartCycle()
        {
            while (true)
            {
                await AcceptPlayer();
            }
        }


        private async Task AcceptPlayer()
        {
            Console.WriteLine("Start to accept player");
            Socket handler = await listener.AcceptAsync();

            var bytes = new byte[1024];

            // Receive the response from the remote device.  
            var bytesRec = handler.Receive(bytes);
            var playerName = Encoding.UTF8.GetString(bytes, 0, bytesRec);

            Console.WriteLine("Nome jogador: {0}", playerName);

            if (players.Count == Arena.MAX_PLAYERS_IN_THE_GAME)
            {
                handler.Send(Encoding.UTF8.GetBytes("The game is already full\n"));
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            else
            {
                players.Add(new PlayerConnection(playerName, arena, handler));
            }
        }

        private void CreateSocket()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(1000);

            Console.WriteLine("Socket connected to {0}", localEndPoint.ToString());
        }
    }

    public enum GameCommands
    {
        EXIT,
        MOVE,
        SCREEN,
        READY,
    }
}

