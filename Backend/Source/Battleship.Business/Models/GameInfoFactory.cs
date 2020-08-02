using System;
using System.Collections.Generic;
using Battleship.Business.Models.Contracts;
using Battleship.Domain.GameDomain.Contracts;
using Battleship.Domain.GridDomain;
using Battleship.Domain.PlayerDomain.Contracts;

namespace Battleship.Business.Models
{
    public class GameInfoFactory : IGameInfoFactory
    {
        private IGridInfoFactory GridInfoFactory;
        private IShipInfoFactory ShipInfoFactory;

        public GameInfoFactory(IGridInfoFactory gridInfoFactory, IShipInfoFactory shipInfoFactory)
        {
            /* Store the grid factory for later use */
            this.GridInfoFactory = gridInfoFactory;
            /* Store the ship factory for later use */
            this.ShipInfoFactory = shipInfoFactory;
        }

        public IGameInfo CreateFromGame(IGame game, Guid playerId)
        {
            /* Create a new gameInfo object */
            GameInfo newGameInfo = new GameInfo();
            /* Copy the id of the game to the gameInfo object id*/
            newGameInfo.Id = game.Id;
            /* Get player from game */
            IPlayer player = game.GetPlayerById(playerId);
            /* Get bombs loaded of player */
            newGameInfo.HasBombsLoaded = player.HasBombsLoaded;
            /* Get the grid of player for creating a grid info object*/
            newGameInfo.OwnGrid = this.GridInfoFactory.CreateFromGrid(player.Grid);
            /* Get the fleet of player for creating a ship object*/
            newGameInfo.OwnShips = this.ShipInfoFactory.CreateMultipleFromFleet(player.Fleet);

            /* Get the opponent from the game */
            IPlayer opponent = game.GetOpponent(player);
            /* Get the grid of opponent for creating a grid info object*/
            newGameInfo.OpponentGrid = this.GridInfoFactory.CreateFromGrid(opponent.Grid);

            if (game.Settings.MustReportSunkenShip == true)
            {
                /* Get the fleet of opponent for creating a ship object*/
                newGameInfo.SunkenOpponentShips = this.ShipInfoFactory.CreateMultipleFromSunkenShipsOfFleet(opponent.Fleet);
            }
            else {
                /* Return an empty ship list */
                newGameInfo.SunkenOpponentShips = new List<IShipInfo>();
            }
            /* The game is ready to start if both players have positioned their ships on the grid */
            newGameInfo.IsReadyToStart = player.Fleet.IsPositionedOnGrid && opponent.Fleet.IsPositionedOnGrid;
            return newGameInfo;
        }
    }
}