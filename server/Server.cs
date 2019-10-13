using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class Server
    {
        private Arena arena;
        private const int MAX_PLAYERS_IN_THE_GAME = 4;
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

            if (players.Count == MAX_PLAYERS_IN_THE_GAME)
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

        class PlayerConnection
        {
            private Socket socket;
            private Player player;

            public PlayerConnection(string playerName, Arena arena, Socket socket)
            {
                player = new Player(playerName, arena);
                this.socket = socket;
                StartCycle().ContinueWith(t => Console.WriteLine("Player died"));
            }

            private async Task StartCycle()
            {
                var task = new Task(() =>
                {
                    string command = "";

                    while (!command.ToLower().StartsWith("exit"))
                    {
                        var bytes = new byte[1024];
                        var bytesRec = socket.Receive(bytes);

                        command = Encoding.UTF8.GetString(bytes, 0, bytesRec);

                        try
                        {
                            ProcessCommand(command);
                            string screen = $"{GameCommands.SCREEN}: {player.GetScreenAsString()}";
                            socket.Send(Encoding.UTF8.GetBytes(screen));
                        }
                        catch (Exception ex)
                        {
                            socket.Send(Encoding.UTF8.GetBytes("invalid-command\n"));
                            Console.Write("Stack: ");
                            Console.WriteLine(ex);
                        }
                    }

                    socket.Send(Encoding.UTF8.GetBytes("goodbye\n"));
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                });

                task.Start();

                await task;
            }

            private void ProcessCommand(string command)
            {
                var parsedCommand = command.ToUpper();

                if (parsedCommand.StartsWith(GameCommands.MOVE.ToString()))
                {
                    string direction = command.Split(":")[1];

                    if (direction == null)
                    {
                        throw new ArgumentException($"Invalid command: {command}");
                    }

                    MovementDirection parsedDirection = (MovementDirection)Enum.Parse(typeof(MovementDirection), direction.Trim(), true);

                    player.Move(parsedDirection);
                }
                else if (parsedCommand.StartsWith(GameCommands.EXIT.ToString()))
                {
                    player.Exit();
                }
                else
                {
                    throw new ArgumentException($"Invalid command: {command}");
                }
            }
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

