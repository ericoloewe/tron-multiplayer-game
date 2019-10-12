using System;
using System.Collections.Generic;

namespace server
{
    class Player
    {
        public IList<Point> Trace { get; private set; }
        public Point Point { get; private set; }
        public string Name { get; }
        private Arena arena;

        internal Player(string playerName, Arena arena)
        {
            Name = playerName;
            Point = new Point();
            this.arena = arena;
            arena.AddPlayer(this);
        }

        internal void Exit()
        {
            throw new NotImplementedException();
        }

        internal string GetScreenAsString()
        {
            throw new NotImplementedException();
        }

        internal void Move(MovementDirection direction)
        {
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
        }
    }

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    enum MovementDirection
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
    }
}
