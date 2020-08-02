using System;
using System.Collections.Generic;
using Battleship.Business.Models.Contracts;
using Battleship.Domain.FleetDomain.Contracts;

namespace Battleship.Business.Models
{
    public class ShipInfoFactory : IShipInfoFactory 
    {
        public IList<IShipInfo> CreateMultipleFromFleet(IFleet fleet)
        {
            IList<IShipInfo> shipInfoList = new List<IShipInfo>(); 
            IList<IShip> shipList = fleet.GetAllShips();

            /* Go through each ship in the ship list of the fleet and create a ship info object.*/
            foreach (var ship in shipList)
            {
                shipInfoList.Add(new ShipInfo(ship));            
            }
            return shipInfoList;
        }

        public IList<IShipInfo> CreateMultipleFromSunkenShipsOfFleet(IFleet fleet)
        {
            IList<IShipInfo> sunkenShipInfoList = new List<IShipInfo>();
            IList<IShip> sunkenShipList = fleet.GetSunkenShips();

            /* Go through each sunken ship in the sunken ship list of the fleet and create a ship info object.*/
            foreach (var sunkenShip in sunkenShipList)
            {
                sunkenShipInfoList.Add(new ShipInfo(sunkenShip));
            }
            return sunkenShipInfoList;
        }
    }
}