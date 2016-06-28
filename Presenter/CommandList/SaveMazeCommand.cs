using System.IO;
using Wpf.ATP.Project.Model;
using Wpf.ATP.Project.View;

namespace Wpf.ATP.Project.Presenter
{
    /// <summary>
    /// Save Maze - Creates a compression of the maze and
    /// saves the file on the disk with a given file path
    /// </summary>
    internal class SaveMazeCommand : ACommand
    {
        /// <summary>
        /// SaveMazeCommand command constructor
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="view">View</param>
        public SaveMazeCommand(IModel model, IView view)
            : base(model, view)
        {
            // Empty Constructor
        }

        /// <summary>
        /// Initates the saveMaze command - Compresses & Saves the data
        /// </summary>
        /// <param name="parameters">Maze name, File path</param>
        public override void DoCommand(params string[] parameters)
        {
            if (parameters.Length != 2)
            {
                m_View.errorOutput("ERROR: Invalid number of parameters.\n" + "Correct use: " + getInformation() + "\n");
                return;
            }
            else if (!fileExists(parameters[1]))
            {
                m_View.errorOutput("ERROR: The path input is not available or incorrect.\n");
            }
            else if (m_model.saveMaze(parameters[0], parameters[1]))
            {
                m_View.Output("Maze saved successfuly\nPath: | " + parameters[1] + " |\n");
            }
            else
            {
                m_View.errorOutput("ERROR: Unable to save the maze.\n");
            }
        }

        /// <summary>
        /// Returns true if the file is found
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>True/False whether the file exists with a given path</returns>
        private bool fileExists(string filePath)
        {
            char[] characters = new char[1];

            while ((filePath.Length > 0) && (!filePath.EndsWith("\\")))
            {
                filePath = filePath.Remove(filePath.Length - 1);
            }
            if (filePath.Length == 0)
                return true;
            else
                return Directory.Exists(filePath);
        }

        /// <summary>
        /// The string of the command
        /// </summary>
        /// <returns>String with the command details</returns>
        public override string getInformation()
        {
            return "saveMaze <maze_name> <file_path>";
        }

        /// <summary>
        /// Returns the name of the command
        /// </summary>
        /// <returns>String with the command name</returns>
        public override string GetName()
        {
            return "savemaze";
        }
    }
}