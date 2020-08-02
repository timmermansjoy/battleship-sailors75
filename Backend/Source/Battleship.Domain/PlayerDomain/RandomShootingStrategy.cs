using System;
using Battleship.Domain.GameDomain;
using Battleship.Domain.GridDomain;
using Battleship.Domain.GridDomain.Contracts;
using Battleship.Domain.PlayerDomain.Contracts;

namespace Battleship.Domain.PlayerDomain
{
    public class RandomShootingStrategy : IShootingStrategy
    {
        private IGrid opponentGrid;

        public RandomShootingStrategy(GameSettings settings, IGrid opponentGrid)
        {
            /* Store opponent grid */
            this.opponentGrid = opponentGrid;
            //The GameSettings parameter will only be needed when you implement certain extra's. But you must leave it. Otherwise some tests will not compile...
        }

        public GridCoordinate DetermineTargetCoordinate()
        {
            GridCoordinate coord;
            IGridSquare square;
            bool freeGridSquareFound = false;

            /* Check status of all grid squares */
            for (int i = 0; i < opponentGrid.Size; i++)
            {
                for (int j = 0; j < opponentGrid.Size; j++)
                {
                    if (opponentGrid.Squares[i, j].Status == GridSquareStatus.Untouched)
                    {
                        /* Break out of loop */
                        freeGridSquareFound = true;
                        break;
                    }
                }
            }

            if (freeGridSquareFound == false)
            {
                /* no free (=untouched) grid square found */
                throw new ApplicationException("Computer has shot all cells in grid");
            }

            do
            {
                /* Generate a random grid coordinate */
                coord = GridCoordinate.CreateRandom(opponentGrid.Size);
                square = opponentGrid.GetSquareAt(coord);
            }
            while (square.Status != GridSquareStatus.Untouched);
            return coord;
        }

        public void RegisterShotResult(GridCoordinate target, ShotResult shotResult)
        {
            //No need do do anything here. Smarter shooting strategies will care more about the result of a shot...
        }
    }
}