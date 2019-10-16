using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace server
{
    class Arena
    {
        public int Width { get; } = 80;
        public int Height { get; } = 80;
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
            Point point = player.Position;

            matrix[point.X][point.Y] = point;
        }

        public override string ToString()
        {
            var colums = matrix.Select(m => $"{string.Join(",", m.ToList())}").ToArray();

            return $"{string.Join("\n", colums)}";
        }
    }
}
