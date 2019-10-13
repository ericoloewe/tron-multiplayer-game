using System;

namespace game
{
    class GameClient
    {
        public Point[][] Screen { get; private set; }

        internal void Connect()
        {
            throw new NotImplementedException();
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
