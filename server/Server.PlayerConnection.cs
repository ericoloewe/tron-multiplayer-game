﻿using System;
using System.Net.Sockets;
using System.Text;

namespace server
{
    partial class Server
    {
        internal class PlayerConnection : IDisposable
        {
            public bool IsConnected { get { return handler.Connected; } }

            private Socket handler;

            internal PlayerConnection(Socket handler)
            {
                this.handler = handler;
            }

            internal string Receive()
            {
                string message;
                int bytesRec;
                var bytes = new byte[10240];

                lock (handler)
                {
                    bytesRec = handler.Receive(bytes);
                }

                message = Encoding.UTF8.GetString(bytes, 0, bytesRec);

                return message.Trim();
            }

            internal void Send(string message)
            {
                lock (handler)
                {
                    handler.Send(Encoding.ASCII.GetBytes($"{message}\n"));
                }
            }

            public void Dispose()
            {
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
    }
}

