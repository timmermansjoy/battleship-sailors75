using System;
using Battleship.Domain.FleetDomain.Contracts;
using Battleship.Domain.GridDomain;
using Battleship.Domain.GridDomain.Contracts;

namespace Battleship.Domain.FleetDomain
{
    public class Ship : IShip
    {
        public IGridSquare[] Squares { get; set; }

        public ShipKind Kind { get; }

        public bool HasSunk {
            get
            {
                int nrOfSquaresHit = 0;
                if (Squares == null)
                {
                    /* If ship has no position yet, return false */
                    return false;
                }
                else
                {
                    /* Loop through all squares of the ship and find out how many have been hit */
                    for (int index = 0; index < Squares.Length; index++)
                    {
                        if (Squares[index].Status == GridSquareStatus.Hit)
                        {
                            nrOfSquaresHit++;
                        }
                    }
                    return (nrOfSquaresHit == Kind.Size);
                }
            }
        }

        public Ship(ShipKind kind)
        {
            /* The type of ship */
            Kind = kind;
            /* Set Squares to null*/
            Squares = null;
        }
        public void ShipHitByBomb(IGridSquare sender)
        {
            sender.Status = GridSquareStatus.Hit;
        }

        public void PositionOnGrid(IGridSquare[] squares)
        {
            if (Squares != null)
            {
                /* Remove bomb handler from old position */
                for (int index = 0; index < Squares.Length; index++)
                {
                    Squares[index].OnHitByBomb -= ShipHitByBomb;
                }
            }
            /* Add bomb handler to new position */
            for (int index = 0; index < squares.Length; index++)
            {
                squares[index].OnHitByBomb += ShipHitByBomb; 
            }
            /* Store new ship position */
            Squares = squares;
        }

        public bool CanBeFoundAtCoordinate(GridCoordinate coordinate)
        {
            /* Check if squares is null = ship not yet positioned on grid */
            if (Squares == null)
            {
                return false;
            }

            /* Check each square if its coordinates matches the supplied coordinates */
            for (int index=0;index< Squares.Length;index++)
            {
                if ((Squares[index].Coordinate.Row == coordinate.Row)
                    &&
                    (Squares[index].Coordinate.Column == coordinate.Column)
                )
                {
                    /* A match was found: stop immediately */
                    return true;
                }
            }
            /* No match found. */
            return false;
        }
    }
}