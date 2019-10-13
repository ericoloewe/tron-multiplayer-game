using System;
using System.Collections.Generic;

namespace server
{
    class Arena
    {
        public int Width { get; } = 800;
        public int Height { get; } = 800;
        private IList<Player> players = new List<Player>();
        private Point[][] matrix;

        public Arena()
        {
            matrix = new Point[Width][];

            for (int i = 0; i < Width; i++)
            {
                matrix[i] = new Point[Height];
            }
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public void Move(Player player)
        {
            Point point = player.Point;

            matrix[point.X][point.Y] = point;
        }
    }
}
