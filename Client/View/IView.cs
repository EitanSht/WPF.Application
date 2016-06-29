using Client.Presenter;
using System.Collections.Generic;

namespace Client.View
{
    public delegate void func();

    /// <summary>
    /// Interface that will be implemented in order to use the View
    /// as part of the MVP module
    /// </summary>
    internal interface IView
    {
        event func ViewChanged;
        event func ViewStart;

        /// <summary>
        /// Starts the view - the user console (CLI)
        /// </summary>
        void Start();

        /// <summary>
        /// Sets the console commands
        /// </summary>
        /// <param name="commands">Console commands dictionary type object</param>
        void setCommands(Dictionary<string, ICommand> commands);

        /// <summary>
        /// Stream output method - Prints to the screen using a message box
        /// </summary>
        /// <param name="output">String with data to be presented</param>
        void Output(string output);

        /// <summary>
        /// Error Stream output method - Prints to the screen using a message box
        /// </summary>
        /// <param name="errorOutput">String with data to be presented</param>
        void errorOutput(string errorOutput);

        /// <summary>
        /// Displays the current maze
        /// </summary>
        /// <param name="winMaze">3dMaze - WPF mode</param>
        /// <param name="floor">Current floor to be displayed</param>
        void printMaze(WinMaze winMaze, int floor);

        /// <summary>
        /// Returns the current command title
        /// </summary>
        /// <returns>String - Command title</returns>
        string getUserCommandName();

        /// <summary>
        /// Returns the details of the current command
        /// </summary>
        /// <returns>String Array - Command Details</returns>
        string[] getUserCommand();

        /// <summary>
        /// Returns all available commands
        /// </summary>
        /// <returns>Dictionary type object with all the commands</returns>
        Dictionary<string, ICommand> getAllCommands();

        /// <summary>
        /// Exists safely from the current View
        /// </summary>
        void exit();

        /// <summary>
        /// Injects a given solution into the WinMaze object
        /// </summary>
        /// <param name="solutionCoordinates">List of string arrys - The solution</param>
        void injectSolution(List<string[]> solutionCoordinates);
    }
}