using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    class Player
    {
        public string Name { get; }
        public Point Point { get; }
        public IList<Point> Trace { get; set; }
        public Arena Arena { get; set; }

        public Player(string playerName)
        {
            this.Name = playerName;
            this.Point = new Point();
        }

        public void Move(MovementDirection direction)
        {
            if (direction == MovementDirection.DOWN && Point.Y < Arena.Height)
            {
                Point.Y++;
            }
            else if (direction == MovementDirection.LEFT && Point.X > 0)
            {
                Point.X--;
            }
            else if (direction == MovementDirection.RIGHT && Point.X < Arena.Width)
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
