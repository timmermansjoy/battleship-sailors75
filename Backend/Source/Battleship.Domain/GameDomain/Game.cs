using System;
using Battleship.Domain.GameDomain.Contracts;
using Battleship.Domain.GridDomain;
using Battleship.Domain.PlayerDomain;
using Battleship.Domain.PlayerDomain.Contracts;

namespace Battleship.Domain.GameDomain
{
    public class Game : IGame
    {
        public Guid Id { get; }
        public GameSettings Settings { get; }

        public IPlayer Player1 { get; }
        public IPlayer Player2 { get; }
        public bool IsStarted { get; private set; }

        internal Game(GameSettings settings, IPlayer player1, IPlayer player2)
        {
            /* Copy the settings into the game. */
            this.Settings = settings;
            /* Store the players. */
            this.Player1 = player1;
            this.Player2 = player2;
            /* Mark game as not started yet. */
            this.IsStarted = false;
            /* Create an identifier for the game. */
            this.Id = Guid.NewGuid();
        }

        public Result Start()
        {
            /* Find the player in the game */
            if (Player1 != null && Player2!=null)
            {
                if (Player1.Fleet.IsPositionedOnGrid == true && Player2.Fleet.IsPositionedOnGrid == true)
                {
                    /* player 1 should have his bombs reloaded */
                    Player1.ReloadBombs();
                    this.IsStarted = true;
                    return Result.CreateSuccessResult();
                }
                else
                {
                    return Result.CreateFailureResult("player not ready");
                }
            }
            else
            {
                return Result.CreateFailureResult("player is null");
            }
        }

        public ShotResult ShootAtOpponent(Guid shooterPlayerId, GridCoordinate coordinate)
        {
            /* Check if the game is started */
            if (this.IsStarted == true)
            {
                /* Get the player */
                IPlayer player = GetPlayerById(shooterPlayerId);

                if (player==null)
                {
                    /* No bombs loaded */
                    return ShotResult.CreateMisfire("Player unknown");
                }

                if (player.HasBombsLoaded == false)
                {
                    /* No bombs loaded */
                    return ShotResult.CreateMisfire("No bombs loaded");
                }

                /* Get opponent */
                IPlayer opponent = GetOpponent(player);

                /* Shoot at the opponent */
                ShotResult result = player.ShootAt(opponent, coordinate);

                /* Reload bombs of opponent */
                opponent.ReloadBombs();

                /* Check if opponent is computer */
                if (opponent is ComputerPlayer)
                {
                    /* Upcast IPlayer to ComputerPlayer */
                    ComputerPlayer computer = (ComputerPlayer)opponent;

                    /* Let the computer */
                    computer.ShootAutomatically(player);

                    /* Reload bombs of player in case of computer */
                    player.ReloadBombs();
                }
                return result;
            }
            else {
                return ShotResult.CreateMisfire("game not started");
            }
        }

        public IPlayer GetPlayerById(Guid playerId)
        {
            /* Return the right player, based on the supplied identifier. */
            if (this.Player1!= null && this.Player1.Id.CompareTo(playerId) == 0)
            {
                return this.Player1;
            }
            else if (this.Player2 != null && this.Player2.Id.CompareTo(playerId) == 0)
            {
                return this.Player2;
            }
            else
            {
                //throw new ApplicationException("unknown player");
                return null;
            }
        }

        public IPlayer GetOpponent(IPlayer player)
        {
            /* Return the opponent player, based on the supplied identifier. */
            if (this.Player1.Id.CompareTo(player.Id) ==0)
            {
                return this.Player2;
            }
            if (this.Player2.Id.CompareTo(player.Id) == 0)
            {
                return this.Player1;
            }
            else
            {
                //throw new ApplicationException("unknown player");
                return null;
            }

        }
    }
}