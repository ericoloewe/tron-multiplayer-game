using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace game
{
    partial class GameClient
    {
        private static readonly int TIME_TO_UPDATE_SCREEN_IN_MS = 500;
        public Point[][] Screen { get; private set; }
        public string PlayerName { get; private set; }
        public Action OnScreenChange { private get; set; }
        public bool HasStarted { get; private set; }
        public bool HasFinished { get; private set; } = false;

        private GameClientConnection clientConnection;
        private Queue<string> commands = new Queue<string>();

        internal void Connect()
        {
            clientConnection = new GameClientConnection();
            StartComunicationCycle().ContinueWith(t => Console.WriteLine("The cicle finished!"));
        }

        internal void StartGame()
        {
            commands.Enqueue("start");
        }

        internal void Ready(string playerName)
        {
            if (string.IsNullOrEmpty(playerName))
            {
                throw new ArgumentException("You have to fill your name first!");
            }
            else
            {
                PlayerName = playerName;
                commands.Enqueue(playerName);
            }
        }

        internal void Move(string pressedKey)
        {
            if (!HasStarted)
            {
                throw new InvalidOperationException("You have to start first!");
            }

            commands.Enqueue($"move: {pressedKey.ToLower()}");
        }

        private void ProcessServerMessage(string message)
        {
            var preparedMessage = message.ToLower();

            if (preparedMessage.Contains("started"))
            {
                HasStarted = true;
            }
            else if (preparedMessage.StartsWith("bem vindo"))
            {
                Console.WriteLine("Receive Bem vindo message");
            }
            else if (preparedMessage.StartsWith("invalid-command"))
            {
                Console.WriteLine("Comando invalido!");
            }
            else
            {
                FormScreen(message);
            }
        }

        private async Task StartComunicationCycle()
        {
            var task = new Task(() =>
            {
                while (!HasFinished)
                {
                    var nextCommand = GetNextCommand();
                    clientConnection.Send(nextCommand);

                    var response = clientConnection.Receive();

                    ProcessServerMessage(response);
                    Thread.Sleep(TIME_TO_UPDATE_SCREEN_IN_MS);
                }
            });

            task.Start();
            await task;
        }

        private string GetNextCommand()
        {
            var command = "screen";

            if (commands.Count > 0)
            {
                command = commands.Dequeue();
            }

            return command;
        }
    }
}
