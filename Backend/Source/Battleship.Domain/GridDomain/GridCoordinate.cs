using System;

namespace Battleship.Domain.GridDomain
{
    public class GridCoordinate
    {
        public int Row { get; }
        public int Column { get; }

        public GridCoordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public static GridCoordinate CreateRandom(int gridSize)
        {
            int row;
            int col;
            Random rand = new Random();
            row = rand.Next(0, gridSize);
            col = rand.Next(0, gridSize);
            return new GridCoordinate(col,row);
        }

        public bool IsOutOfBounds(int gridSize)
        {
            /* Check if the coordinate is off the grid. */
            if (Row >= gridSize || Row< 0 || Column >= gridSize || Column <0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public GridCoordinate GetNeighbor(Direction direction)
        {
            int row = Row;
            int col = Column;

            row += direction.YStep;
            col += direction.XStep;

            return new GridCoordinate(row, col);
        }

        public GridCoordinate GetOtherEnd(Direction direction, int distance)
        {
            int row = Row;
            int col = Column;

            row += direction.YStep* distance;
            col += direction.XStep* distance;

            return new GridCoordinate(row,col);
        }

        #region overrides
        //DO NOT TOUCH THIS METHODS IN THIS REGION!

        public override string ToString()
        {
            return $"({Row},{Column})";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GridCoordinate);
        }

        protected bool Equals(GridCoordinate other)
        {
            if (ReferenceEquals(other, null)) return false;
            return Row == other.Row && Column == other.Column;
        }

        public static bool operator ==(GridCoordinate a, GridCoordinate b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.Equals(b);
        }

        public static bool operator !=(GridCoordinate a, GridCoordinate b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        #endregion
    }

}