using MazeGenerators;
using Search;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace Client
{
    /// <summary>
    /// Interface that will be implemented in order to use the Model
    /// as part of the MVP module
    /// </summary>
    internal interface IModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Generate a 3d Maze - Generates a new maze with given user input
        /// </summary>
        /// <param name="name">Maze Name</param>
        /// <param name="levels">How many levels to the maze</param>
        /// <param name="columns">How many columns to the maze</param>
        /// <param name="rows">How many rows to the maze</param>
        void generate3dMaze();

        /// <summary>
        /// Returns an existing maze by its name
        /// </summary>
        /// <param name="mazeName">Maze name</param>
        /// <returns>Dictionary type object representing a maze</returns>
        Dictionary<int, Maze2d> getMaze(string mazeName);

        /// <summary>
        /// Save Maze - Creates a compression of the maze and
        /// saves the file on the disk with a given file path
        /// </summary>
        /// <param name="mazeName">Maze name</param>
        /// <param name="path">File Name</param>
        /// <returns>True if successful saving the maze</returns>
        bool saveMaze(string mazeName, string path);

        /// <summary>
        /// Load Maze - Loads a maze to the memory with a given file path
        /// from the user.
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <param name="mazeName">Name of the maze</param>
        void loadMaze(string path, string mazeName);

        /// <summary>
        /// Solve Maze - Solves the given maze with a given
        /// solving algorithm - BFS/DFS
        /// </summary>
        /// <param name="mazeName">Maze name</param>
        /// <param name="solvingAlgorithm">Solving algorithm - BFS/DFS</param>
        void solveMaze(string solvingAlgorithm);

        /// <summary>
        /// Display Solution - Displays the solution of the command
        /// </summary>
        /// <param name="mazeName">Maze name</param>
        /// <returns>Solution type object containing the solution if exists</returns>
        Solution displaySolution(string mazeName);

        /// <summary>
        /// Returns true if the maze is found on the memory (dictionary)
        /// </summary>
        /// <param name="mazeName">Maze name</param>
        /// <returns>True/False if the maze is found</returns>
        bool mazeExists(string mazeName);

        /// <summary>
        /// Exit - Quits the program safely & closes
        /// all open thread and processes
        /// </summary>
        void exit();

        /// <summary>
        /// Stops safely all running threads
        /// </summary>
        void Stop();

        /// <summary>
        /// Returns the WinMaze from the dictionary
        /// </summary>
        /// <param name="name">Name of the maze</param>
        /// <returns>WinMaze if fond, Null if not</returns>
        WinMaze getWinMaze(string name);

        /// <summary>
        /// Move left in the maze command
        /// </summary>
        /// <param name="maze">Current WinMaze maze</param>
        void moveLeft();

        /// <summary>
        /// Move right in the maze command
        /// </summary>
        /// <param name="maze">Current WinMaze maze</param>
        void moveRight();

        /// <summary>
        /// Move back in the maze command
        /// </summary>
        /// <param name="maze">Current WinMaze maze</param>
        void moveBack();

        /// <summary>
        /// Move forward in the maze command
        /// </summary>
        /// <param name="maze">Current WinMaze maze</param>
        void moveForward();

        /// <summary>
        /// Move down in the maze command
        /// </summary>
        /// <param name="maze">Current WinMaze maze</param>
        void moveDown();

        /// <summary>
        /// Move up in the maze command
        /// </summary>
        /// <param name="maze">Current WinMaze maze</param>
        void moveUp();

        /// <summary>
        /// Transfered from View-Model: To display the solution
        /// </summary>
        void showSolution();

        /// <summary>
        /// Transfered from View-Model: To hide the solution
        /// </summary>
        void hideSolution();

        /// <summary>
        /// Transfered from View-Model: To create a new maze
        /// </summary>
        void newCanvas();

        /// <summary>
        /// Transfered from View-Model: To load a maze
        /// </summary>
        void loadClicked();

        /// <summary>
        /// Transfered from View-Model: To save a maze
        /// </summary>
        void saveClicked();

        #region Properties Decleration
        string LevelData { get; set; }
        string ColumnData { get; set; }
        string RowData { get; set; }
        string CellSizeData { get; set; }
        Canvas CurrentMazeCanvas { get; set; }
        Canvas SecondaryMazeCanvas { get; set; }
        string Output { get; set; }
        string ErrorOutput { get; set; }
        #endregion
    }
}