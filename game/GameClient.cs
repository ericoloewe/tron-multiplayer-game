using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace game
{
    class GameClient
    {
        private readonly int serverPort = 8080;
        public Point[][] Screen { get; private set; }
        public string PlayerName { get; private set; }
        public Socket sender;

        internal void Connect()
        {
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var endpoint = new IPEndPoint(ipAddress, serverPort);
            sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(endpoint);
        }

        internal void StartGame(string playerName)
        {
            PlayerName = playerName;
            sender.Send(Encoding.ASCII.GetBytes(playerName));
        }
    }

    class Point
    {
        public string PlayerName { get; }
        public PointType Type { get; }
    }

    enum PointType
    {
        PLAYER,
        TRACE
    }
}
