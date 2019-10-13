using System;
using System.Collections.Generic;

namespace server
{
    class Player
    {
        public IList<Point> Trace { get; private set; } = new List<Point>();
        public Point Position { get; private set; }
        public string Name { get; }
        private Arena arena;

        public Player(string playerName, Arena arena)
        {
            Name = playerName;
            Position = new Point(0, 0, this);
            this.arena = arena;
            arena.AddPlayer(this);
            arena.Move(this);
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

        public string GetScreenAsString()
        {
            return arena.ToString();
        }

        public void Move(MovementDirection direction)
        {
            Trace.Add(Position);

            Point nextPoint = (Point)Position.Clone();

            if (direction == MovementDirection.DOWN && Position.Y < arena.Height)
            {
                nextPoint.Y++;
            }
            else if (direction == MovementDirection.LEFT && Position.X > 0)
            {
                nextPoint.X--;
            }
            else if (direction == MovementDirection.RIGHT && Position.X < arena.Width)
            {
                nextPoint.X++;
            }
            else if (direction == MovementDirection.UP && Position.Y > 0)
            {
                nextPoint.Y--;
            }

            Position = nextPoint;
            arena.Move(this);
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

            return $"{{{playerOrTrace};{Player.Name};{X};{Y}}}";
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
