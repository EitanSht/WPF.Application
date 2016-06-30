using MazeGenerators;
using System;
using System.Collections.Generic;

namespace Client
{
    /// <summary>
    /// WinMaze class, represents the WPF version of
    /// the 3d maze
    /// </summary>
    [Serializable]
    public class WinMaze
    {
        private Maze3d mMaze;
        private string mName;
        private int mCellSize = 10;
        private int mPosX = 0;
        private int mPosY = 0;
        private int mPosZ = 0;
        private bool solutionExists = false;
        private List<string[]> mSolutionCoordinates;
        private Dictionary<int, Cell[,]> mMazeCells = new Dictionary<int, Cell[,]>();

        /// <summary>
        /// WinMaze Constructor
        /// </summary>
        /// <param name="maze">3d maze</param>
        /// <param name="name">Name of the maze</param>
        /// <param name="cellSize">Size of the cells</param>
        public WinMaze(Maze3d maze, string name, int cellSize)
        {
            mName = name;
            mMaze = maze;
            mCellSize = cellSize;
            mSolutionCoordinates = new List<string[]>();
            for (int level = 0; level < maze.MyHeight; level++)
            {
                mMazeCells[level] = new Cell[maze.MyColumns, maze.MyRows];
                for (int row = 0; row < maze.MyRows; row++)
                {
                    for (int column = 0; column < maze.MyColumns; column++)
                    {
                        mMazeCells[level][column, row] = maze.getMazeByFloor(level).getCell(column, row);
                    }
                }
            }
            PosZ = mMaze.MyHeight - 1;
        }

        /// <summary>
        /// Returns the name of the maze
        /// </summary>
        /// <returns>Name of the maze</returns>
        public string getName()
        {
            return mName;
        }

        /// <summary>
        /// Returns the 3d maze
        /// </summary>
        /// <returns></returns>
        [STAThread]
        public Maze3d getMaze()
        {
            return mMaze;
        }

        /// <summary>
        /// Sets the solution of the maze
        /// </summary>
        /// <param name="solutionCoordinates">List of string array - Solution</param>
        public void setSolutionCoordinates(List<string[]> solutionCoordinates)
        {
            if (solutionCoordinates != null)
            {
                mSolutionCoordinates = solutionCoordinates;
                solutionExists = true;
            }
        }

        /// <summary>
        /// Returns tru if the solution exists
        /// </summary>
        /// <returns>True if the solution exists</returns>
        public bool isSolutionExists()
        {
            return solutionExists;
        }

        /// <summary>
        /// Returns the solutions, by a level key
        /// </summary>
        /// <returns>Dictionary type object that contains the solution</returns>
        public Dictionary<int, int[,]> solutionCoordinatesByLevel()
        {
            if (isSolutionExists())
            {
                Dictionary<int, int[,]> solutionDictByLevel = new Dictionary<int, int[,]>();

                for (int level = 0; level < mMaze.MyHeight; level++)
                {
                    solutionDictByLevel[level] = new int[mMaze.MyColumns, mMaze.MyRows];
                    for (int column = 0; column < mMaze.MyColumns; column++)
                    {
                        for (int row = 0; row < mMaze.MyRows; row++)
                        {
                            solutionDictByLevel[level][column, row] = 0;
                        }
                    }
                }

                string[] zxy = new string[3];
                foreach (string[] coordinates in mSolutionCoordinates)
                {
                    zxy = coordinates;
                    int z = Int32.Parse(coordinates[0]);
                    int x = Int32.Parse(coordinates[1]);
                    int y = Int32.Parse(coordinates[2]);
                    solutionDictByLevel[z][x, y] = 1;
                }
                return solutionDictByLevel;
            }
            else
                return null;
        }

        /// <summary>
        /// Clears the solution of the maze
        /// </summary>
        public void clearSolution()
        {
            if (isSolutionExists())
            {
                mSolutionCoordinates = new List<string[]>();
                for (int level = 0; level < mMaze.MyHeight; level++)
                {
                    mMazeCells[level] = new Cell[mMaze.MyColumns, mMaze.MyRows];
                    for (int column = 0; column < mMaze.MyColumns; column++)
                    {
                        for (int row = 0; row < mMaze.MyRows; row++)
                        {
                            mMazeCells[level][column, row] = mMaze.getMazeByFloor(level).getCell(column, row);
                        }
                    }
                }
            }
        }

        #region Properties

        public int PosX
        {
            get { return mPosX; }
            set { mPosX = value; }
        }

        public int PosY
        {
            get { return mPosY; }
            set { mPosY = value; }
        }

        public int PosZ
        {
            get { return mPosZ; }
            set { mPosZ = value; }
        }

        public int CellSize
        {
            get { return mCellSize; }
            set { mCellSize = value; }
        }

        public Dictionary<int, Cell[,]> AllCells
        {
            get { return mMazeCells; }
        }

        #endregion Properties
    }
}