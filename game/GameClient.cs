using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace game
{
    class GameClient
    {
        public Point[][] Screen { get; private set; }
        public string PlayerName { get; private set; }
        public Action OnScreenChange { private get; set; }
        public bool HasStarted { get; private set; }

        private readonly int serverPort = 8080;
        private Socket sender;

        internal void Connect()
        {
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var endpoint = new IPEndPoint(ipAddress, serverPort);
            sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(endpoint);
        }

        internal void StartGame(string playerName)
        {
            if (sender == null)
            {
                throw new InvalidOperationException("You have to connect first!");
            }
            else if (string.IsNullOrEmpty(playerName))
            {
                throw new ArgumentException("Yout have to fill your name first!");
            }
            else
            {
                PlayerName = playerName;
                sender.Send(Encoding.ASCII.GetBytes(playerName));
                ReceiveAndFormScreen(true);
                HasStarted = true;
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

        private void ReceiveAndFormScreen(bool? forceReset = false)
        {
            var bytes = new byte[10240];
            var bytesRec = sender.Receive(bytes);

            var screenStr = Encoding.UTF8.GetString(bytes, 0, bytesRec);

            string[] rows = screenStr.Split("\n");

            if (forceReset.HasValue)
            {
                ResetScreen(rows.Length);
            }

            for (int i = 0; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split(",");

                for (int j = 0; j < columns.Length; j++)
                {
                    Screen[i][j] = Point.FromText(columns[j].Replace("SCREEN: ", ""));
                }
            }

            OnScreenChange.Invoke();
        }

        private void ResetScreen(int size)
        {
            Screen = new Point[size][];

            for (int i = 0; i < size; i++)
            {
                Screen[i] = new Point[size];
            }
        }
    }

    class Point
    {
        public string PlayerName { get; }
        public PointType Type { get; }

        private Point(string playerName, string type)
        {
            PlayerName = playerName;
            Type = (PointType)Enum.Parse(typeof(PointType), type, true);
        }

        internal static Point FromText(string pointInfo)
        {
            Point point = null;

            if (pointInfo.Trim().Length > 0)
            {
                string[] info = pointInfo.Split(";");

                point = new Point(info[1], info[0]);
            }

            return point;
        }
    }

    enum PointType
    {
        PLAYER,
        TRACE
    }
}
