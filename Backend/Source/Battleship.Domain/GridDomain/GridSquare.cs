using System;
using Battleship.Domain.GridDomain.Contracts;

namespace Battleship.Domain.GridDomain
{
    public class GridSquare : IGridSquare
    {
        public GridSquareStatus Status { get; set; }

        public GridCoordinate Coordinate { get; }

        public int NumberOfBombs { get; private set; }

        /// <summary>
        /// When a grid square is hit by a bomb (HitByBomb method is called), the OnHitByBomb event will be invoked.
        /// The square being hit is the sender of the event.
        /// </summary>
        public event HitByBombHandler OnHitByBomb;

        public GridSquare(GridCoordinate coordinate)
        {
            /* Store the coordinates of the grid square */
            Coordinate = coordinate;

            /* Set the number of bombs to 0*/
            NumberOfBombs = 0;

            /* The status of the grid square is untouched */
            Status = GridSquareStatus.Untouched;

            /* Store the on bomb handler */
            OnHitByBomb += GridSquareHitByBomb;
        }

        public void GridSquareHitByBomb(IGridSquare sender)
        {
            /* Change the status of the grid square when touched by a bomb */
            sender.Status = GridSquareStatus.Miss;
        }

        public void HitByBomb()
        {
            /* Increase the number of bombs*/
            NumberOfBombs++;
            /* Call the event handler */
            OnHitByBomb?.Invoke(this);
        }
    }

    public delegate void HitByBombHandler(IGridSquare sender);
}