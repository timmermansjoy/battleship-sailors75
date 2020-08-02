using System;
using Battleship.Business.Models.Contracts;
using Battleship.Domain.FleetDomain;
using Battleship.Domain.FleetDomain.Contracts;
using Battleship.Domain.GridDomain;

namespace Battleship.Business.Models
{
    public class ShipInfo : IShipInfo
    {
        public GridCoordinate[] Coordinates { get; }

        public ShipKind Kind { get; }

        public bool HasSunk { get;}

        public ShipInfo(IShip ship)
        {
            /* Converts a ship into a ship info object */
            
            /* Store type of ship */
            this.Kind = ship.Kind;

            /* Store sunk status of ship */
            this.HasSunk = ship.HasSunk;

            /* Check if ship has alreday been positioned on grid */
            if (ship.Squares != null) 
            {
                /* Copy the coordinates from the grid squares */
                this.Coordinates = new GridCoordinate[ship.Squares.Length];
                for (int index = 0; index < ship.Squares.Length; index++)
                {
                    Coordinates[index] = new GridCoordinate(ship.Squares[index].Coordinate.Row, ship.Squares[index].Coordinate.Column);
                }
            }
        }
    }
}