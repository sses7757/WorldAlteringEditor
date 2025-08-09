using System;
using System.Collections.Generic;
using TSMapEditor.GameMath;

namespace TSMapEditor.Models
{
    public enum TubeDirection
    {
        None = -1,
        First = 0,
        NorthEast = 0,
        East = 1,
        SouthEast = 2,
        South = 3,
        SouthWest = 4,
        West = 5,
        NorthWest = 6,
        North = 7,

        Max = 7
    }

    /// <summary>
    /// A tunnel tube on the map.
    /// </summary>
    public class Tube
    {
        public Tube()
        {

        }

        public Tube(Point2D entryPoint, Point2D exitPoint, TubeDirection unitInitialFacing, List<TubeDirection> directions)
        {
            EntryPoint = entryPoint;
            ExitPoint = exitPoint;
            UnitInitialFacing = unitInitialFacing;
            Directions = directions;
        }

        public Point2D EntryPoint { get; set; }
        public Point2D ExitPoint { get; set; }
        public TubeDirection UnitInitialFacing { get; set; }
        public List<TubeDirection> Directions { get; set; } = [];
        public bool Pending { get; set; }


        public void ShiftPosition(int xChange, int yChange)
        {
            EntryPoint += new Point2D(xChange, yChange);
            ExitPoint += new Point2D(xChange, yChange);
        }


        private static TubeDirection GetOpposingDirection(TubeDirection direction)
        {
            return direction switch
            {
                TubeDirection.NorthEast => TubeDirection.SouthWest,
                TubeDirection.East => TubeDirection.West,
                TubeDirection.SouthEast => TubeDirection.NorthWest,
                TubeDirection.South => TubeDirection.North,
                TubeDirection.SouthWest => TubeDirection.NorthEast,
                TubeDirection.West => TubeDirection.East,
                TubeDirection.NorthWest => TubeDirection.SouthEast,
                TubeDirection.North => TubeDirection.South,
                TubeDirection.None => TubeDirection.None,
                _ => throw new ArgumentException("Unknown tube direction: " + direction),
            };
        } 

        public Tube GetReversedTube()
        {
            var reversedTube = new Tube
            {
                EntryPoint = ExitPoint,
                ExitPoint = EntryPoint
            };

            for (int i = Directions.Count - 1; i > -1; i--)
            {
                if (Directions[i] == TubeDirection.None)
                    continue;

                reversedTube.Directions.Add(GetOpposingDirection(Directions[i]));
            }

            if (reversedTube.Directions[^1] != TubeDirection.None)
                reversedTube.Directions.Add(TubeDirection.None); // Tube direction list must end in -1 aka no direction

            reversedTube.UnitInitialFacing = reversedTube.Directions[0];
            return reversedTube;
        }
    }
}
