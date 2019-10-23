using System;

namespace game
{
    public class Point
    {
        public string PlayerName { get; }
        public PointType Type { get; }

        private Point(string playerName, string type)
        {
            PlayerName = playerName;
            Type = (PointType)Enum.Parse(typeof(PointType), type, true);
        }

        internal static Point FromText(string pointInfo)
        {
            Point point = null;

            if (pointInfo.Trim().Length > 0)
            {
                string[] info = pointInfo.Split(";");

                point = new Point(info[1], info[0]);
            }

            return point;
        }
    }

    public enum PointType
    {
        PLAYER,
        TRACE
    }
}
