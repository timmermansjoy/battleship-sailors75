using System;
using Battleship.Business.Models.Contracts;
using Battleship.Domain.GridDomain;
using Battleship.Domain.GridDomain.Contracts;

namespace Battleship.Business.Models
{
    public class GridInfoFactory : IGridInfoFactory
    {
        public IGridInfo CreateFromGrid(IGrid grid)
        {
            GridInfo gridInfo = new GridInfo();
            gridInfo.Size = grid.Size;
            gridInfo.Squares = new GridSquareInfo[gridInfo.Size][];
            for (int row = 0; row < gridInfo.Size; row++)
            {
                gridInfo.Squares[row] = new GridSquareInfo[gridInfo.Size];
                for (int col = 0; col < gridInfo.Size; col++)
                {
                    GridCoordinate c = new GridCoordinate(row, col);
                    gridInfo.Squares[row][col] = new GridSquareInfo(grid.Squares[c.Row, c.Column]); /* not clear what the bug is */
                }
            }
            return gridInfo;   
        }
    }
}