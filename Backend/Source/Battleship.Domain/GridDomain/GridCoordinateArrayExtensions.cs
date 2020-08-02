using System;

namespace Battleship.Domain.GridDomain
{
    public static class GridCoordinateArrayExtensions
    {
        public static bool HasAnyOutOfBounds(this GridCoordinate[] coordinates, int gridSize)
        {
            for (int i = 0; i < coordinates.Length; i++) {
                if (coordinates[i].IsOutOfBounds(gridSize) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool AreAligned(this GridCoordinate[] coordinates)
        {
            return (AreHorizontallyAligned(coordinates) || AreVerticallyAligned(coordinates));
        }

        public static bool AreHorizontallyAligned(this GridCoordinate[] coordinates)
        {
            int row = coordinates[0].Row;
            for (int i = 0; i < coordinates.Length; i++)
            {
                if (coordinates[i].Row != row)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool AreVerticallyAligned(this GridCoordinate[] coordinates)
        {
            int col = coordinates[0].Column;
            for (int i = 0; i < coordinates.Length; i++)
            {
                if (coordinates[i].Column != col)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool AreDiagonallyAligned(this GridCoordinate[] coordinates)
        {
            int row = coordinates[0].Row;
            int col = coordinates[0].Column;
            int stepY = coordinates[1].Row    - coordinates[0].Row;
            int stepX = coordinates[1].Column - coordinates[0].Column;

            for (int i = 1; i < coordinates.Length; i++)
            {
                if (
                    ((coordinates[i].Row - coordinates[i - 1].Row) != stepY)
                    ||
                    ((coordinates[i].Column - coordinates[i - 1].Column) != stepX)
                  )
                {
                    return false;
                }
            }
            return true;
        }

        public static bool AreLinked(this GridCoordinate[] coordinates)
        {
            int curRow = coordinates[0].Row;
            int curCol = coordinates[0].Column;
            if (AreVerticallyAligned(coordinates) == true)
            {
                for (int i = 1; i < coordinates.Length; i++)
                {
                    if (    (coordinates[i].Row != curRow + 1) 
                        &&  (coordinates[i].Row != curRow - 1)
                    )
                    {
                        return false;
                    }
                    else
                    {
                        curRow = coordinates[i].Row;
                    }
                }
                return true;
            }
            else if (AreHorizontallyAligned(coordinates) == true)
            {
                for (int i = 1; i < coordinates.Length; i++)
                {
                    if (   (coordinates[i].Column != curCol + 1) 
                        && (coordinates[i].Column != curCol - 1)
                       )
                    {
                        return false;
                    }
                    else
                    {
                        curCol = coordinates[i].Column;
                    }
                }
                return true;
            }
            else {
                for (int i = 1; i < coordinates.Length; i++)
                {
                    if (   (coordinates[i].Row != curRow + 1)
                        && (coordinates[i].Row != curRow - 1)
                        && (coordinates[i].Column != curCol + 1)
                        && (coordinates[i].Column != curCol - 1)
                        )
                        {
                        return false;
                    }
                    else
                    {
                        curRow = (coordinates[i]).Row;
                        curCol = (coordinates[i]).Column;
                    }
                }
                return true;
            }
        }

        public static string Print(this GridCoordinate[] coordinates)
        {
            return $"[{string.Join<GridCoordinate>(", ", coordinates)}]";
        }
    }
}