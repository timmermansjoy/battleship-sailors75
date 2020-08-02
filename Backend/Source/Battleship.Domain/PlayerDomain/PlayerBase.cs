using System;
using Battleship.Domain.FleetDomain;
using Battleship.Domain.FleetDomain.Contracts;
using Battleship.Domain.GameDomain;
using Battleship.Domain.GridDomain;
using Battleship.Domain.GridDomain.Contracts;
using Battleship.Domain.PlayerDomain.Contracts;

namespace Battleship.Domain.PlayerDomain
{
    public abstract class PlayerBase : IPlayer
    {
        public Guid Id { get; }
        public string NickName { get; }
        public IGrid Grid { get; }
        public IFleet Fleet { get; }

        private bool mustReportSunkenShip;

        public bool HasBombsLoaded { 
            get
            { return nrOfBombsLoaded > 0; } 
        }

        private int nrOfBombsLoaded;

        protected PlayerBase(Guid id, string nickName, GameSettings gameSettings)
        {
            /* Store ID for the user */
            this.Id = id;

            /* Stote the nickname of the user */
            this.NickName = nickName;

            /* Create the grid */
            this.Grid = new Grid(gameSettings.GridSize);

            /* Create an empty fleet*/
            this.Fleet = new Fleet(gameSettings.AllowDeformedShips);

            /* No bombs loaded yet */
            this.nrOfBombsLoaded = 0;

            this.mustReportSunkenShip = gameSettings.MustReportSunkenShip;
        }

        public void ReloadBombs()
        {
            /* Load the bombs */
            this.nrOfBombsLoaded = 1;
        }

        public ShotResult ShootAt(IPlayer opponent, GridCoordinate coordinate)
        {
            /* Get the grid of the opponent & shoot */
            IGridSquare square = opponent.Grid.Shoot(coordinate);

            /* Bomb has been shot */
            this.nrOfBombsLoaded=0 ;

            /* The grid square informas about the result of the shot */
            if (square.Status == GridSquareStatus.Hit)
            {
                /* Ship hit */
                return ShotResult.CreateHit(opponent.Fleet.FindShipAtCoordinate(coordinate), mustReportSunkenShip);
            }
            else if (square.Status == GridSquareStatus.Miss)
            {
                /* Missed */
                return ShotResult.CreateMissed();
            }
            else
            {
                return ShotResult.CreateMisfire("out of bound");
            }
        }
    }
}