using System;
using System.Collections.Generic;
using Battleship.Domain.FleetDomain.Contracts;
using Battleship.Domain.GridDomain;
using Battleship.Domain.GridDomain.Contracts;

namespace Battleship.Domain.FleetDomain
{
    public class Fleet : IFleet
    {
        /* Add a list for storing the 5 ships */
        private Dictionary<ShipKind, IShip> shipList;

        private Boolean allowDeformedShips = false;

        /* Add constructor to add the 5 ships */
        public Fleet(bool allowDeformedShips=false) {
            /* Add the 5 ships */
            shipList = new Dictionary<ShipKind, IShip>();
            shipList.Add(ShipKind.Battleship, new Ship(ShipKind.Battleship));   //5 positions
            shipList.Add(ShipKind.Carrier, new Ship(ShipKind.Carrier));         //4 positions
            shipList.Add(ShipKind.Destroyer, new Ship(ShipKind.Destroyer));     //3 positions
            shipList.Add(ShipKind.Submarine, new Ship(ShipKind.Submarine));     //3 positions
            shipList.Add(ShipKind.PatrolBoat, new Ship(ShipKind.PatrolBoat));   //2 positions

            /* Mark if ships can be deformed */
            this.allowDeformedShips = allowDeformedShips;
        }

        public bool IsPositionedOnGrid {
            get
            {
                /* Go through all ships */
                foreach (KeyValuePair<ShipKind, IShip> aShip in this.shipList)
                {
                    if (aShip.Value.Squares == null)
                    {
                        /* If the ship Squares are null it has not been positioned */
                        return false;
                    }
                }
                return true;
            }        
        }

        public Result TryMoveShipTo(ShipKind kind, GridCoordinate[] segmentCoordinates, IGrid grid)
        {
            /* Check if the segmentcoordinates have the same length as the ship */
            if (segmentCoordinates.Length != kind.Size) {
                return Result.CreateFailureResult("Ship length is not ok");
            }

            /* Check if the segmentcoordinates are vertically or horizontally aligned */
            if (this.allowDeformedShips == false)
            {
                if ((segmentCoordinates.AreHorizontallyAligned() == false) && (segmentCoordinates.AreVerticallyAligned() == false))
                {
                    return Result.CreateFailureResult("Ship coordinates are not aligned");
                }
            }
            else 
            {
                if ((segmentCoordinates.AreHorizontallyAligned() == false) && (segmentCoordinates.AreVerticallyAligned() == false) && (segmentCoordinates.AreDiagonallyAligned() == false))
                {
                    return Result.CreateFailureResult("Ship coordinates are not aligned");
                }
            }
            /* Check if the segmentcoordinates are linked */
            if (segmentCoordinates.AreLinked() == false)
            {
                return Result.CreateFailureResult("Ship coordinates are not linked");
            }
            /* Check if the segmentcoordinates are not out of bound (ship not fully in grid)*/
            for (int index = 0; index < kind.Size; index++)
            {
                if (segmentCoordinates[index].IsOutOfBounds(grid.Size) == true)
                {
                    return Result.CreateFailureResult("Ship out of bound");
                }
            }
            /* Check if this ship collides with another */
            foreach (KeyValuePair<ShipKind, IShip> otherShip in this.shipList)
            {
                /* Check that this ship is different from the others */
                if (otherShip.Value.Kind != kind)
                {
                    /* Go through the segmentcoordinates of this ship and check if the others have been placed on the same coordinates */
                    for (int index = 0; index < segmentCoordinates.Length; index++)
                    {
                        if (otherShip.Value.CanBeFoundAtCoordinate(segmentCoordinates[index]) == true)
                        {
                            return Result.CreateFailureResult("Other ship at same coordinates");
                        }
                    }
                }
            }

            /* Assign the segmentcoordinates to the ship = place the ship on the grid.*/
            IGridSquare[] gridsquares = new IGridSquare[kind.Size];
            for (int index = 0; index < kind.Size; index++)
            {
                /* Get the grid square with the right coordinates from the grid object.*/
                gridsquares[index] = grid.GetSquareAt(segmentCoordinates[index]);
            }

            /*Assign the grid squares to the ship */
            this.shipList[kind].PositionOnGrid(gridsquares);

            /* Everything went well, return success */
            return Result.CreateSuccessResult();
        }

        public void RandomlyPositionOnGrid(IGrid grid, bool allowDeformedShips = false)
        {
            GridCoordinate[] coordinates;
            IList<IShip> shipList = GetAllShips();
            Result result;

            foreach (IShip ship in shipList)
            {
                /* Position ship randomly */
                coordinates = ship.Kind.GenerateRandomSegmentCoordinates(grid.Size, allowDeformedShips);
                /* Try to position ship */
                result=TryMoveShipTo(ship.Kind, coordinates, grid);

                while (result.IsFailure)
                {
                    /* Position ship randomly */
                    coordinates = ship.Kind.GenerateRandomSegmentCoordinates(grid.Size, allowDeformedShips);
                    /* Try to position ship again*/
                    result = TryMoveShipTo(ship.Kind, coordinates, grid);
                }
            }
        }

        public IShip FindShipAtCoordinate(GridCoordinate coordinate)
        {
            /* Go through all the ships */
            foreach (KeyValuePair<ShipKind, IShip> aShip in this.shipList)
            {
                /* Look if a ship is positioned at a specific coordinate */
                if (aShip.Value.CanBeFoundAtCoordinate(coordinate) == true)
                {
                    /* ship found, return ship object */
                    return aShip.Value;
                }
            }
            /* no ship found */
            return null;
        }

        public IList<IShip> GetAllShips()
        {
            List<IShip> shipList = new List<IShip>();
            /* Create a list of ship objects.*/
            foreach (KeyValuePair<ShipKind, IShip> ship in this.shipList)
            {
                shipList.Add(ship.Value);
            }
            return shipList;
        }

        public IList<IShip> GetSunkenShips()
        {
            List<IShip> sunkenShipList = new List<IShip>();
            
            /* Create a list of sunken ship objects.*/
            foreach (KeyValuePair<ShipKind, IShip> ship in this.shipList)
            {
                /* Add the ship object to the list if it is sunk.*/
                if (ship.Value.HasSunk == true)
                {
                    sunkenShipList.Add(ship.Value);
                }
            }
            return sunkenShipList;
        }
    }
}