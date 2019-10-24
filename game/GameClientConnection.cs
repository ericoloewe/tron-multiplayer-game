using System.Net;
using System.Net.Sockets;
using System.Text;

namespace game
{
    internal class GameClientConnection
    {
        private readonly string serverIp = "127.0.0.1";
        private readonly int serverPort = 8080;
        private Socket sender;
        public bool HasMessage { get { return sender.Available > 0; } }

        internal GameClientConnection()
        {
            var ipAddress = IPAddress.Parse(serverIp);
            var endpoint = new IPEndPoint(ipAddress, serverPort);

            sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(endpoint);
        }

        internal string Receive()
        {
            string message;
            int bytesRec;
            var bytes = new byte[10240];

            lock (sender)
            {
                bytesRec = sender.Receive(bytes);
            }

            message = Encoding.UTF8.GetString(bytes, 0, bytesRec);

            return message;
        }

        internal void Send(string message)
        {
            lock (sender)
            {
                sender.Send(Encoding.ASCII.GetBytes(message));
            }
        }
    }
}
