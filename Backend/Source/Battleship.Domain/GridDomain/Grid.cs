using System;
using Battleship.Domain.GridDomain.Contracts;

namespace Battleship.Domain.GridDomain
{
    public class Grid : IGrid
    {
        public IGridSquare[,] Squares { get; }

        public int Size { get; }

        public Grid(int size)
        {
            /* Set the size of the grid.*/
            Size = size;

            /* Create an array for the grid squares.*/
            this.Squares = new GridSquare[size,size];
            for (int row = 0; row < size; row++)
            {
               for (int col = 0; col < size; col++)
                {
                    /* Create a grid square object for each cell of the grid. */
                    this.Squares[row,col] = new GridSquare(new GridCoordinate(row,col));
                }
            }
        }

        public IGridSquare GetSquareAt(GridCoordinate coordinate)
        {
            /* Return the grid square with the requested coordinates. */
            return this.Squares[coordinate.Row, coordinate.Column];
        }

        public IGridSquare Shoot(GridCoordinate coordinate)
        {
            /* Check if the coordinate is out of bound. */
            if (coordinate.IsOutOfBounds(Size) == false)
            {
                /* Call the HitByBomb method of the grid square.*/
                this.Squares[coordinate.Row, coordinate.Column].HitByBomb();
                return this.Squares[coordinate.Row, coordinate.Column];
            }
            else
            {
                /* Return exception when coordinate is not with the grid.*/
                throw new System.ApplicationException("Coordinate is out of bound");
            }
        }
    }
}