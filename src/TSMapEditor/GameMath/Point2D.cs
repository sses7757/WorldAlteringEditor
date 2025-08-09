using Microsoft.Xna.Framework;
using Rampastring.Tools;
using System;
using TSMapEditor.Models;

namespace TSMapEditor.GameMath
{
    public struct Point2D(int x, int y)
    {
        public int X = x;
        public int Y = y;

        public static Point2D operator +(Point2D p1, Point2D p2)
        {
            return new Point2D(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point2D operator +(Point2D p)
        {
            return new Point2D(p.X, p.Y);
        }

        public static Point2D operator -(Point2D p1, Point2D p2)
        {
            return new Point2D(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Point2D operator -(Point2D p)
        {
            return new Point2D(-p.X, -p.Y);
        }

        public static Point2D FromXNAPoint(Point point)
            => new(point.X, point.Y);

        public static Point2D Zero => new(0, 0);

        public static Point2D NegativeOne => new(-1, -1);

        public readonly Vector2 ToXNAVector() => new(X, Y);

        public readonly Point ToXNAPoint() => new(X, Y);

        public override readonly int GetHashCode()
        {
            return Y * 1000 + X;
        }

        public override readonly string ToString()
        {
            return X + ", " + Y;
        }

        public static Point2D FromString(string str)
        {
            string[] pointData = str.Split(',');
            if (pointData.Length != 2)
                throw new ArgumentException("Point2D.FromString: Invalid source string " + str);

            int x = Conversions.IntFromString(pointData[0].Trim(), -1);
            int y = Conversions.IntFromString(pointData[1].Trim(), -1);
            return new Point2D(x, y);
        }

        public static bool operator !=(Point2D p1, Point2D p2)
        {
            return !(p1 == p2);
        }

        public static bool operator ==(Point2D p1, Point2D p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public override readonly bool Equals(object obj)
        {
            if (obj is Point2D objAsPoint)
            {
                return objAsPoint == this;
            }

            return false;
        }

        public readonly Point2D NextPointFromTubeDirection(TubeDirection direction)
        {
            return direction switch
            {
                TubeDirection.NorthEast => this + new Point2D(0, -1),
                TubeDirection.East => this + new Point2D(1, -1),
                TubeDirection.SouthEast => this + new Point2D(1, 0),
                TubeDirection.South => this + new Point2D(1, 1),
                TubeDirection.SouthWest => this + new Point2D(0, 1),
                TubeDirection.West => this + new Point2D(-1, 1),
                TubeDirection.NorthWest => this + new Point2D(-1, 0),
                TubeDirection.North => this + new Point2D(-1, -1),
                _ => this,
            };
        }

        public readonly float Angle()
        {
            return (float)Math.Atan2(Y, X);
        }

        /// <summary>
        /// Calculates and returns this point's tile-based distance to another point.
        /// </summary>
        /// <param name="other">The other point.</param>
        public readonly int DistanceTo(Point2D other)
        {
            return Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y));
        }

        public readonly Point2D ScaleBy(double scale) => new((int)(X * scale), (int)(Y * scale));

        public readonly Point2D ScaleBy(float scale) => new((int)(X * scale), (int)(Y * scale));

        public readonly byte[] GetData()
        {
            byte[] buffer = new byte[sizeof(int) * 2];

            byte[] xb = BitConverter.GetBytes(X);
            byte[] yb = BitConverter.GetBytes(Y);
            Array.Copy(xb, buffer, xb.Length);
            Array.Copy(yb, 0, buffer, xb.Length, yb.Length);
            return buffer;
        }
    }
}
