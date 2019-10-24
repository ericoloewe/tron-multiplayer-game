using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace server
{
    partial class Server
    {
        private Arena arena;
        private IList<PlayerProcessor> players = new List<PlayerProcessor>();
        private int port;
        private Socket listener;

        public Server(int port)
        {
            arena = new Arena();
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
            var playerConnection = new PlayerConnection(await listener.AcceptAsync());

            playerConnection.Send("Bem vindo ao jogo, digite seu nome:");

            var playerName = playerConnection.Receive();

            Console.WriteLine("Nome jogador: {0}", playerName);

            if (players.Count == Arena.MAX_PLAYERS_IN_THE_GAME)
            {
                playerConnection.Send("The game is already full");
                playerConnection.Dispose();
            }
            else
            {
                var player = new PlayerProcessor(playerName, arena, playerConnection);

                player.OnStop = () => players.Remove(player);
                player.OnStart = () => StartPlayers();
                players.Add(player);
            }
        }

        private void StartPlayers()
        {
            foreach (var player in players)
            {
                player.Start();
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
        READY,
        SCREEN,
        START,
    }
}

