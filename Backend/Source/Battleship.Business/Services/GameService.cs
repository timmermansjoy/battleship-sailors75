using System;
using System.Data;
using Battleship.Business.Models.Contracts;
using Battleship.Business.Services.Contracts;
using Battleship.Domain;
using Battleship.Domain.FleetDomain;
using Battleship.Domain.FleetDomain.Contracts;
using Battleship.Domain.GameDomain;
using Battleship.Domain.GameDomain.Contracts;
using Battleship.Domain.GridDomain;
using Battleship.Domain.PlayerDomain;
using Battleship.Domain.PlayerDomain.Contracts;

namespace Battleship.Business.Services
{
    public class GameService : IGameService
    {
        private IGameFactory gameFactory;
        private IGameRepository gameRepository;
        private IGameInfoFactory gameInfoFactory;

        public GameService(
            IGameFactory gameFactory,
            IGameRepository gameRepository, 
            IGameInfoFactory gameInfoFactory)
        {
            /* Store these objects for later use in other methods */
            this.gameFactory = gameFactory;
            this.gameRepository = gameRepository;
            this.gameInfoFactory = gameInfoFactory;
        }

        public IGameInfo CreateGameForUser(GameSettings settings, User user)
        {
            /* Create a new game object using the game factory.*/
            IGame newGame = this.gameFactory.CreateNewSinglePlayerGame(settings, user);
            /* Add the new game object to the repository.*/
            this.gameRepository.Add(newGame);
            /* Create a game info object from the newly created game object.*/
            IGameInfo newGameInfo = this.gameInfoFactory.CreateFromGame(newGame, user.Id);
            /* Return the new game info object.*/
            return newGameInfo;
        }

        public Result StartGame(Guid gameId, Guid playerId)
        {
            /* Look up game in repository */
            IGame retrievedGame = this.gameRepository.GetById(gameId);
            /* throws DataNotFoundException when not found */

            if (retrievedGame.GetPlayerById(playerId) == null)
            {
                return Result.CreateFailureResult("no match between player and game");
            }
            return retrievedGame.Start();
        }

        public IGameInfo GetGameInfoForPlayer(Guid gameId, Guid playerId)
        {
            /* Look up game in repository */
            IGame retrievedGame = this.gameRepository.GetById(gameId);
            /* throws DataNotFoundException when not found */

            /* Create a game info object from the retrieved game object.*/
            IGameInfo newGameInfo = this.gameInfoFactory.CreateFromGame(retrievedGame, playerId);
            /* Return the new game info object.*/
            return newGameInfo;
        }

        public Result PositionShipOnGrid(Guid gameId, Guid playerId, ShipKind shipKind, GridCoordinate[] segmentCoordinates)
        {
            /* Look up game in repository */
            IGame retrievedGame = this.gameRepository.GetById(gameId);
            /* throws DataNotFoundException when not found */

            /* Cannot move ships */
            if (retrievedGame.IsStarted == true && retrievedGame.Settings.CanMoveUndamagedShipsDuringGame==false){
                return Result.CreateFailureResult("cannot move ship anymore");
            }

            /* Find the player in the game */
            IPlayer player = retrievedGame.GetPlayerById(playerId);

            if (player != null)
            {
                /* Player is found - get the fleet */
                IFleet fleet = player.Fleet;

                /* Try to position ship on the grid */
                Result result = fleet.TryMoveShipTo(shipKind, segmentCoordinates,player.Grid);

                return result;
            }
            else
            {
                /* Player not found in game */
                throw new ApplicationException("unknown player");
            }
        }

        public ShotResult ShootAtOpponent(Guid gameId, Guid shooterPlayerId, GridCoordinate coordinate)
        {
            IGame retrievedGame;
            IPlayer player;
            IPlayer opponent;

            /* Look up game in repository */
            retrievedGame = gameRepository.GetById(gameId);
            /* throws DataNotFoundException when not found */

            /* Shoot at opponent */
            return retrievedGame.ShootAtOpponent(shooterPlayerId, coordinate);
        }
    }
}