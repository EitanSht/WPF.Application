using MazeGenerators;
using Search;
using System.Collections.Generic;

namespace Client
{
    internal delegate void func();

    /// <summary>
    /// Interface that will be implemented in order to use the Model
    /// as part of the MVP module
    /// </summary>
    internal interface IModel
    {
        event func ModelChanged;

        /// <summary>
        /// Directory Command - Returns file names & paths
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <returns>If path exists - files & directories, if not - error message</returns>
        string[] getDir(string path);

        /// <summary>
        /// Generate a 3d Maze - Generates a new maze with given user input
        /// </summary>
        /// <param name="name">Maze Name</param>
        /// <param name="levels">How many levels to the maze</param>
        /// <param name="columns">How many columns to the maze</param>
        /// <param name="rows">How many rows to the maze</param>
        void generate3dMaze(string name, int levels, int columns, int rows, int size);

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
        /// File Size - Returns the size of the file in bytes
        /// </summary>
        /// <param name="Path">Directory path</param>
        /// <returns>Size in bytes of the given file name in the disc</returns>
        long fileSize(string Path);

        /// <summary>
        /// Solve Maze - Solves the given maze with a given
        /// solving algorithm - BFS/DFS
        /// </summary>
        /// <param name="mazeName">Maze name</param>
        /// <param name="solvingAlgorithm">Solving algorithm - BFS/DFS</param>
        void solveMaze(string mazeName, string solvingAlgorithm);

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
        /// Returns true if there is a solution for the current maze
        /// </summary>
        /// <param name="mazeName">Maze name</param>
        /// <returns>True if solution exists</returns>
        bool solutionExists(string mazeName);

        /// <summary>
        /// Saves the current solution to the disk
        /// </summary>
        void saveSolutionDictionary();

        /// <summary>
        /// Loads the solutions of previous mazes from the disc
        /// </summary>
        void loadSolutionDictionary();

        /// <summary>
        /// Checks if the solution file exists
        /// </summary>
        /// <returns>True if the soultion file exists</returns>
        bool isSolutionFileExists();

        /// <summary>
        /// Saves the maze into the disc
        /// </summary>
        void saveMazeDictionary();

        /// <summary>
        /// Loads all previous mazes
        /// </summary>
        void loadMazeDictionary();

        /// <summary>
        /// Checks if the mazes file exits
        /// </summary>
        /// <returns>True if the mazes file exists</returns>
        bool isMazesFileExists();

        /// <summary>
        /// Unzips the compressed mazes file
        /// </summary>
        /// <param name="directoryPath">Path of the maze dictionary</param>
        void unZipDictionaries(string directoryPath);

        /// <summary>
        /// Compresses (Zip) the current dictionary
        /// </summary>
        /// <param name="directoryPath">Path of the maze dictionary</param>
        void zipDictionaries(string directoryPath);

        /// <summary>
        /// Exit - Quits the program safely & closes
        /// all open thread and processes
        /// </summary>
        void exit();

        /// <summary>
        /// Creates a 'Model Changed' event
        /// </summary>
        void modelEvent();

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
        void moveLeft(WinMaze maze);

        /// <summary>
        /// Move right in the maze command
        /// </summary>
        /// <param name="maze">Current WinMaze maze</param>
        void moveRight(WinMaze maze);

        /// <summary>
        /// Move back in the maze command
        /// </summary>
        /// <param name="maze">Current WinMaze maze</param>
        void moveBack(WinMaze maze);

        /// <summary>
        /// Move forward in the maze command
        /// </summary>
        /// <param name="maze">Current WinMaze maze</param>
        void moveForward(WinMaze maze);

        /// <summary>
        /// Move down in the maze command
        /// </summary>
        /// <param name="maze">Current WinMaze maze</param>
        void moveDown(WinMaze maze);

        /// <summary>
        /// Move up in the maze command
        /// </summary>
        /// <param name="maze">Current WinMaze maze</param>
        void moveUp(WinMaze maze);

        /// <summary>
        /// Returns the current instructions from the Model
        /// </summary>
        /// <returns>String array of instructions</returns>
        string[] getInstructions();
    }
}