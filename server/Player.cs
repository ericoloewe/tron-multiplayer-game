using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    class Player
    {
        private Arena arena;
        public IList<Point> Trace { get; private set; }
        public Point Point { get; private set; }
        public string Name { get; }

        public Player(string playerName, Arena arena)
        {
            this.Name = playerName;
            this.Point = new Point();
            this.arena = arena;
        }

        public void Move(MovementDirection direction)
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

        internal void Exit()
        {
            throw new NotImplementedException();
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
