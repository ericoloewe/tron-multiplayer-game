using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    class Arena
    {
        public static readonly int MAX_PLAYERS_IN_THE_GAME = 4;
        private readonly int cicleTime = 1000;

        public int Width { get; } = 80;
        public int Height { get; } = 80;
        public bool HasStarted { get; private set; }
        public bool HasFinished { get; private set; }

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
            switch (players.Count)
            {
                case 0:
                    {
                        player.Position.X = 0;
                        player.Position.Y = 0;
                        break;
                    }
                case 1:
                    {
                        player.Position.X = Width;
                        player.Position.Y = Height;
                        break;
                    }
                case 2:
                    {
                        player.Position.X = 0;
                        player.Position.Y = Height;
                        break;
                    }
                case 4:
                    {
                        player.Position.X = Width;
                        player.Position.Y = 0;
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException("You can't add more than 4 players");
                    }
            }

            players.Add(player);
            SetPlayerAtMatrix(player);
        }

        internal void Remove(Player player)
        {
            if (HasStarted)
            {
                Finish();
            }
            else
            {
                players.Remove(player);
            }
        }

        public void Move(Player player)
        {
            SetPlayerAtMatrix(player);
        }


        public async Task Start()
        {
            HasStarted = true;
            Console.WriteLine("Starting arena");

            var task = new Task(() => StartCycle());

            task.Start();
            await task;
        }

        public override string ToString()
        {
            var colums = matrix.Select(m => $"{string.Join(",", m.ToList())}").ToArray();

            return $"{string.Join("\n", colums)}";
        }

        private void Finish()
        {
            HasFinished = true;
            throw new NotImplementedException();
        }

        private void KeepMovingPlayers()
        {
            foreach (var player in players)
            {
                player.Move();
            }
        }

        private void StartCycle()
        {
            while (!HasFinished)
            {
                KeepMovingPlayers();

                Thread.Sleep(cicleTime);
            }
        }

        private void SetPlayerAtMatrix(Player player)
        {
            Point point = player.Position;

            matrix[point.X][point.Y] = point;
        }
    }
}
