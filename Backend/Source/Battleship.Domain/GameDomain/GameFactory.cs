using System;
using Battleship.Domain.GameDomain.Contracts;
using Battleship.Domain.PlayerDomain;
using Battleship.Domain.PlayerDomain.Contracts;

namespace Battleship.Domain.GameDomain
{
    public class GameFactory : IGameFactory
    {
        public IGame CreateNewSinglePlayerGame(GameSettings settings, User user)
        {
            /* Create the human player object */
            HumanPlayer player1 = new HumanPlayer(user,settings);
            /* Create shooting strategy object for computer and pass grid of opponent*/
            RandomShootingStrategy strategy = new RandomShootingStrategy(settings, player1.Grid);
            /* Create the computer player object */
            ComputerPlayer player2 = new ComputerPlayer(settings, strategy);
            /* Create the game object */
            Game game = new Game(settings,player1,player2);
            /* Return the game object */
            return game;
        }

        public IGame CreateNewTwoPlayerGame(GameSettings settings, IPlayer player1, IPlayer player2)
        {
            /* Create the game object */
            Game game = new Game(settings, player1, player2);
            /* Return the game object */
            return game;
        }
    }
}