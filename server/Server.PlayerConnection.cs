using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    partial class Server
    {
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
                            string screen = $"{GameCommands.SCREEN}: {player.GetScreenAsString()}\n";
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
}

