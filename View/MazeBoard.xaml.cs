using MazeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.ATP.Project.Model;

namespace Wpf.ATP.Project.View
{
    /// <summary>
    /// Interaction logic for MazeBoard.xaml
    /// </summary>
    public partial class MazeBoard : UserControl
    {
        /// <summary>
        /// MazeBoard Constructor
        /// </summary>
        /// <param name="maze">Current WinMaze</param>
        /// <param name="floor">Current floor</param>
        /// <param name="mazeCellSize">Size of the cells</param>
        /// <param name="showLevel">Show this/lower/upper level</param>
        public MazeBoard(WinMaze maze, int floor, int mazeCellSize, int showLevel)
        {
            InitializeComponent();
            if (showLevel == 0) // Current
                CreateMaze(maze, floor, mazeCellSize);
            if (showLevel == 1) // Up
                CreateMaze(maze, floor + 1, mazeCellSize);
            if (showLevel == -1) // Down
                CreateMaze(maze, floor - 1, mazeCellSize);
        }

        /// <summary>
        /// Creates the maze & displays it on the screen
        /// </summary>
        /// <param name="maze">Current WinMaze</param>
        /// <param name="floor">Current floor</param>
        /// <param name="mazeCellSize">Size of the cells</param>
        private void CreateMaze(WinMaze maze, int floor, int mazeCellSize)
        {
            MazeCell mazeCell;
            Cell cell3d;
            Dictionary<int, int[,]> solutionDictByLevel = new Dictionary<int, int[,]>();
            if (maze.isSolutionExists())
                solutionDictByLevel = maze.solutionCoordinatesByLevel();
            for (int row = 0; row < maze.getMaze().MyRows; row++)
            {
                for (int column = 0; column < maze.getMaze().MyColumns; column++)
                {
                    cell3d = maze.getMaze().getMazeByFloor(floor).getCell(column, row);
                    int[] cellWalls = cell3d.getWallsAroundCell();
                    int isSpecial = 0;
                    if (1 == cell3d.BlockOrEmpty)
                        isSpecial = 1;
                    if (maze.isSolutionExists()) // mark the solution
                    {
                        if (solutionDictByLevel[floor][column, row] == 1)
                        {
                            isSpecial = 5;
                        }
                    }
                    if (column == maze.getMaze().MyColumns - 1)
                        cellWalls[1] = 1;
                    if (row == maze.getMaze().MyRows - 1)
                        cellWalls[3] = 1;
                    if ((column == maze.getMaze().MyColumns - 1) && (row == maze.getMaze().MyRows - 1) && (floor == 0))
                    {
                        cellWalls[1] = 0;
                        isSpecial = 4;
                    }
                    if ((column == 0) && (row == 0) && (floor == maze.getMaze().MyHeight - 1))
                    {
                        cellWalls[0] = 0;
                        isSpecial = 3;
                    }
                    int currX = maze.PosX;
                    int currY = maze.PosY;
                    if ((column == currX) && (row == currY))
                    {
                        isSpecial = 2;
                    }
                    if (column + 1 < maze.getMaze().MyColumns)
                    {
                        Cell cell3dright = maze.getMaze().getMazeByFloor(floor).getCell(column + 1, row);
                        int[] cellWallsright = cell3dright.getWallsAroundCell();
                        if ((cellWallsright[0] == 0) || (cellWalls[1] == 0))
                            cellWalls[1] = 0;
                    }
                    if (row + 1 < maze.getMaze().MyRows)
                    {
                        Cell cell3dforward = maze.getMaze().getMazeByFloor(floor).getCell(column, row + 1);
                        int[] cellWallsforward = cell3dforward.getWallsAroundCell();
                        if ((cellWallsforward[2] == 0) || (cellWalls[3] == 0))
                            cellWalls[3] = 0;
                    }
                    if (isSpecial == 1)
                    {
                        cellWalls[0] = 1; cellWalls[1] = 1; cellWalls[2] = 1; cellWalls[3] = 1; cellWalls[4] = 1; cellWalls[5] = 1;
                    }
                    if (column == 0)
                        cellWalls[0] = 1;
                    if (row == 0)
                        cellWalls[2] = 1;
                    mazeCell = new MazeCell(mazeCellSize, isSpecial, cellWalls[0] == 1, cellWalls[1] == 1, cellWalls[2] == 1, cellWalls[3] == 1, column == 0, row == 0);

                    mazeBoard.Children.Add(mazeCell);
                    Canvas.SetLeft(mazeCell, mazeCellSize * column);
                    Canvas.SetTop(mazeCell, mazeCellSize * row);
                }
            }
        }
    }
}
