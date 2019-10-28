using System;
using System.Collections.Generic;
using System.Net.Sockets;
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
            public Action OnStop { private get; set; } = () => { };
            public Action OnStart { get; internal set; }
            private bool wasStarted = false;
            private bool wasStoped = false;
            private Queue<string> commands = new Queue<string>();

            public PlayerProcessor(string playerName, Arena arena, PlayerConnection connection)
            {
                this.arena = arena;
                player = new Player(playerName, arena);
                this.connection = connection;
                player.OnStop = () => HandleGameStop();
                player.OnDie = () => HandleGameStop();
                StartCycle().ContinueWith(t => HandleGameStop());
            }

            private async Task StartCycle()
            {
                var task = new Task(() =>
                {
                    var command = "";
                    var parsedCommand = command.Trim().ToLower();

                    while (!parsedCommand.StartsWith("exit") || !parsedCommand.StartsWith("stop") || !connection.IsConnected)
                    {
                        try
                        {
                            command = ReceiveAndProcessClientMessage();
                        }
                        catch (SocketException ex)
                        {
                            Console.WriteLine("Stack: ");
                            Console.WriteLine(ex);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Stack: ");
                            Console.WriteLine(ex);
                        }
                    }

                    connection.Send("goodbye");
                    connection.Dispose();
                });

                task.Start();
                await task;
            }

            private string ReceiveAndProcessClientMessage()
            {
                string command;
                string response;

                command = connection.Receive();

                try
                {
                    ProcessCommand(command);

                    response = GetNextCommand();
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("invalid-command of player");
                    response = "invalid-command";
                }

                connection.Send(response);

                return command;
            }

            private string GetNextCommand()
            {
                var command = "";

                if (commands.Count > 0)
                {
                    command = commands.Dequeue();
                }

                return command;
            }

            internal void Start()
            {
                var command = "already-started";

                if (!wasStarted)
                {
                    command = "start";
                }

                commands.Enqueue(command);
            }

            private void SendScreen()
            {
                var screen = player.GetScreenAsString();

                commands.Enqueue(screen);
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

                    player.ChangeDirection(parsedDirection);
                }
                else if (parsedCommand.StartsWith(GameCommands.EXIT.ToString()))
                {
                    player.Die();
                }
                else if (parsedCommand.StartsWith(GameCommands.START.ToString()))
                {
                    StartArena();
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

            private void StartArena()
            {
                if (!arena.CanStart)
                {
                    throw new ArgumentException($"Can't start the game");
                }

                arena.Start().ContinueWith(t => HandleGameStop());
                wasStarted = true;
                OnStart.Invoke();
            }

            private void HandleGameStop()
            {
                if (!wasStoped)
                {
                    commands.Enqueue("stop");
                    wasStoped = true;
                    Console.WriteLine("The game was stopped");
                    OnStop.Invoke();
                }
            }
        }
    }
}

