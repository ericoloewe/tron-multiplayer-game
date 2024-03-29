﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    class Arena
    {
        public static readonly int MAX_PLAYERS_IN_THE_GAME = 4;

        public int Width { get; } = 80;
        public int Height { get; } = 80;
        public bool HasStarted { get; private set; }
        public bool HasFinished { get; private set; }
        public bool CanStart { get { return players.Count >= 2; } }

        private readonly int cicleTime = 500;
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
                        player.Position.X = Width - 1;
                        player.Position.Y = Height - 1;
                        break;
                    }
                case 2:
                    {
                        player.Position.X = 0;
                        player.Position.Y = Height - 1;
                        break;
                    }
                case 4:
                    {
                        player.Position.X = Width - 1;
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

        internal void Kill(Player player)
        {
            if (!player.IsDead)
                player.Die();

            if (HasStarted)
                Stop();
            else
            {
                var point = player.Position;

                matrix[point.X][point.Y] = null;
                players.Remove(player);
            }
        }

        public void Update(Player player)
        {
            var point = player.Position;

            if (point.X < 0 || point.Y < 0 || point.X > Width - 1 || point.Y > Height - 1)
            {
                Kill(player);
                throw new ArgumentException($"Invalid next point {point}");
            }
            else if (matrix[point.X][point.Y] != null)
            {
                Kill(player);
                throw new InvalidOperationException("There is another player at point");
            }
            else
            {
                SetPlayerAtMatrix(player);
            }
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

            return $"{string.Join("<br>", colums)}";
        }

        private void Stop()
        {
            HasFinished = true;

            foreach (var player in players)
            {
                player.Stop();
            }
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

            if (Width - 1 < point.X || Height - 1 < point.Y)
            {
                throw new ArgumentException($"Invalid point {point}");
            }

            matrix[point.X][point.Y] = point;
        }
    }
}
