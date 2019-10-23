using System;

namespace game
{
    partial class GameClient
    {
        private static readonly int TIME_TO_UPDATE_SCREEN_IN_MS = 500;
        public Point[][] Screen { get; private set; }
        public string PlayerName { get; private set; }
        public Action OnScreenChange { private get; set; }
        public Action<string> OnConnect { private get; set; }
        public bool HasStarted { get; private set; }
        public bool HasFinished { get; private set; } = false;

        private GameClientConnection clientConnection;

        internal void Connect()
        {
            clientConnection = new GameClientConnection();
            ReceiveWelcomeMessage();
        }

        private void ReceiveWelcomeMessage()
        {
            var message = clientConnection.Receive();

            if (OnConnect != null)
            {
                OnConnect.Invoke(message);
            }
        }

        internal void StartGame()
        {
            clientConnection.Send("start");
            var errorMessage = clientConnection.Receive();

            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new InvalidOperationException($"There is an error to start {errorMessage}");
            }
            else
            {
                HasStarted = true;
            }
        }

        internal void Ready(string playerName)
        {
            if (string.IsNullOrEmpty(playerName))
            {
                throw new ArgumentException("Yout have to fill your name first!");
            }
            else
            {
                PlayerName = playerName;
                clientConnection.Send(playerName);
                StartScreenCycle().ContinueWith(t => Console.WriteLine("The cicle finished!"));
            }
        }

        internal void Move(string pressedKey)
        {
            if (!HasStarted)
            {
                throw new InvalidOperationException("You have to start first!");
            }

            clientConnection.Send($"move: {pressedKey.ToLower()}");
            ReceiveAndFormScreen();
        }
    }
}
