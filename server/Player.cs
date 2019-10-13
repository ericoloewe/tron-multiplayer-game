using System;
using System.Collections.Generic;

namespace server
{
    class Player
    {
        public IList<Point> Trace { get; private set; } = new List<Point>();
        public Point Point { get; private set; }
        public string Name { get; }
        private Arena arena;

        public Player(string playerName, Arena arena)
        {
            Name = playerName;
            Point = new Point(this);
            this.arena = arena;
            arena.AddPlayer(this);
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

        public string GetScreenAsString()
        {
            throw new NotImplementedException();
        }

        public void Move(MovementDirection direction)
        {
            Trace.Add(Point);

            if (direction == MovementDirection.DOWN && Point.Y < arena.Height)
            {
                Point.Y++;
            }
            else if (direction == MovementDirection.LEFT && Point.X > 0)
            {
                Point.X--;
            }
            else if (direction == MovementDirection.RIGHT && Point.X < arena.Width)
            {
                Point.X++;
            }
            else if (direction == MovementDirection.UP && Point.Y > 0)
            {
                Point.Y--;
            }

            arena.Move(this);
        }
    }

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Player Player { get; }

        public Point(Player player)
        {
            Player = player;
        }

        public bool IsTrace()
        {
            return this == Player.Point;
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
