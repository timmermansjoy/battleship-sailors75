using System;
using Battleship.Domain.GameDomain;
using Battleship.Domain.GridDomain;
using Battleship.Domain.PlayerDomain.Contracts;

namespace Battleship.Domain.PlayerDomain
{
    public class ComputerPlayer : PlayerBase
    {
        private IShootingStrategy shootingStrategy;

        public ComputerPlayer(GameSettings settings, IShootingStrategy shootingStrategy) : base(Guid.NewGuid(), "Computer", settings)
        {
            this.Fleet.RandomlyPositionOnGrid(this.Grid, settings.AllowDeformedShips);
            this.shootingStrategy = shootingStrategy;
        }

        public void ShootAutomatically(IPlayer opponent)
        {
            /* Determine grid coordinate */
            GridCoordinate coord = this.shootingStrategy.DetermineTargetCoordinate();

            /* Shoot at opponent */
            ShotResult result = this.ShootAt(opponent,coord);

            /* Store shot result in strategy */
            this.shootingStrategy.RegisterShotResult(coord, result);
        }
    }
}