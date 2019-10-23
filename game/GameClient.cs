using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

        private readonly int serverPort = 8080;
        private Socket sender;

        internal void Connect()
        {
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var endpoint = new IPEndPoint(ipAddress, serverPort);
            sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(endpoint);
            ReceiveWelcomeMessage();
        }

        private void ReceiveWelcomeMessage()
        {
            var bytes = new byte[1024];
            var bytesRec = sender.Receive(bytes);

            if (OnConnect != null)
            {
                var message = Encoding.UTF8.GetString(bytes, 0, bytesRec);

                OnConnect.Invoke(message);
            }
        }

        internal void StartGame()
        {
            sender.Send(Encoding.ASCII.GetBytes("start"));
            var bytes = new byte[10240];
            var bytesRec = sender.Receive(bytes);
            var errorMessage = Encoding.UTF8.GetString(bytes, 0, bytesRec);

            if (string.IsNullOrEmpty(errorMessage))
            {
                Console.WriteLine($"There is an error to start {errorMessage}");
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
                sender.Send(Encoding.ASCII.GetBytes(playerName));
                StartScreenCycle().ContinueWith(t => Console.WriteLine("The cicle finished!"));
            }
        }

        internal void Move(string pressedKey)
        {
            if (!HasStarted)
            {
                throw new InvalidOperationException("You have to start first!");
            }

            sender.Send(Encoding.ASCII.GetBytes($"move: {pressedKey.ToLower()}"));
            ReceiveAndFormScreen();
        }
    }
}
