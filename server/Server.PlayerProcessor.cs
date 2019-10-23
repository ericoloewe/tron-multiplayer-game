using System;
using System.Threading.Tasks;

namespace server
{
    partial class Server
    {
        class PlayerProcessor
        {
            private PlayerConnection connection;
            private Player player;
            private Arena arena;

            public PlayerProcessor(string playerName, Arena arena, PlayerConnection connection)
            {
                this.arena = arena;
                player = new Player(playerName, arena);
                this.connection = connection;
                StartCycle().ContinueWith(t => Console.WriteLine("Player died"));
            }

            private async Task StartCycle()
            {
                var task = new Task(() =>
                {
                    string command = "";

                    while (!command.ToLower().StartsWith("exit"))
                    {
                        command = connection.Receive();

                        try
                        {
                            ProcessCommand(command);
                        }
                        catch (ArgumentException)
                        {
                            Console.WriteLine("invalid-command of player");
                            connection.Send("invalid-command");
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Stack: ");
                            Console.WriteLine(ex);
                        }
                    }

                    connection.Send("goodbye");
                    connection.Dispose();
                });

                task.Start();
                await task;
            }

            private void SendScreen()
            {
                var screen = player.GetScreenAsString();

                connection.Send(screen);
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
                else if (parsedCommand.StartsWith(GameCommands.START.ToString()))
                {
                    if (!arena.CanStart)
                    {
                        throw new ArgumentException($"Can't start the game");
                    }

                    arena.Start().ContinueWith(t => Console.WriteLine("The game was stopped"));
                }
                else if (parsedCommand.StartsWith(GameCommands.SCREEN.ToString()))
                {
                    SendScreen();
                }
                else
                {
                    throw new ArgumentException($"Invalid command: {command}");
                }
            }
        }
    }
}

