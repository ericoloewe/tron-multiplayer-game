﻿using System;
using System.Collections.Generic;

namespace server
{
    class Player
    {
        public IList<Point> Trace { get; private set; } = new List<Point>();
        public Point Position { get; private set; }
        public string Name { get; }
        public Action OnStop { private get; set; }
        public Action OnDie { private get; set; }
        public bool IsDead { get; private set; }

        private Arena arena;
        private MovementDirection? currentDirection;
        private bool isStopped;

        public Player(string playerName, Arena arena)
        {
            Name = playerName;
            Position = new Point(0, 0, this);
            this.arena = arena;
            arena.AddPlayer(this);
        }

        public void Die()
        {
            if (!IsDead)
            {
                IsDead = true;
                arena.Kill(this);
                OnDie.Invoke();
            }
        }

        public string GetScreenAsString()
        {
            return arena.ToString();
        }

        public void ChangeDirection(MovementDirection nextDirection)
        {
            currentDirection = nextDirection;
        }

        public void Move()
        {
            currentDirection = GetNextDefaultDirection();
            Trace.Add(Position);

            Point nextPoint = (Point)Position.Clone();

            if (currentDirection == MovementDirection.DOWN && Position.Y < arena.Height)
            {
                nextPoint.Y++;
            }
            else if (currentDirection == MovementDirection.LEFT && Position.X > 0)
            {
                nextPoint.X--;
            }
            else if (currentDirection == MovementDirection.RIGHT && Position.X < arena.Width)
            {
                nextPoint.X++;
            }
            else if (currentDirection == MovementDirection.UP && Position.Y > 0)
            {
                nextPoint.Y--;
            }

            Position = nextPoint;
            arena.Update(this);
        }

        internal void Stop()
        {
            if (!isStopped)
            {
                isStopped = true;
                Die();
                OnStop.Invoke();
            }
        }

        private MovementDirection GetNextDefaultDirection()
        {
            var nextDirection = MovementDirection.DOWN;

            if (currentDirection != null)
            {
                nextDirection = currentDirection.Value;
            }
            else if (Position.Y == arena.Height - 1)
            {
                nextDirection = MovementDirection.UP;
            }

            return nextDirection;
        }
    }

    class Point : ICloneable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Player Player { get; }

        public Point(int x, int y, Player player)
        {
            X = x;
            Y = y;
            Player = player;
        }

        public bool IsTrace()
        {
            return this == Player.Position;
        }

        public override string ToString()
        {
            var playerOrTrace = IsTrace() ? "PLAYER" : "TRACE";

            return $"{playerOrTrace};{Player.Name}";
        }

        public object Clone()
        {
            return new Point(X, Y, Player);
        }
    }

    enum MovementDirection
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
    }
}
