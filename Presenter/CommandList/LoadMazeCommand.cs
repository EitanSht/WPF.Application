using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf.ATP.Project.Model;
using Wpf.ATP.Project.View;

namespace Wpf.ATP.Project.Presenter
{
    /// <summary>
    /// Load Maze - Loads a maze to the memory with a given file path
    /// from the user
    /// </summary>
    class LoadMazeCommand : ACommand
    {
        /// <summary>
        /// LoadMazeCommand command constructor
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="view">View</param>
        public LoadMazeCommand(IModel model, IView view)
            : base(model, view)
        {
            // Empty Constructor
        }

        /// <summary>
        /// Initates the loadMaze command with the parameters
        /// </summary>
        /// <param name="parameters">File path and Maze name</param>
        public override void DoCommand(params string[] parameters)
        {
            if (parameters.Length != 2)
            {
                m_View.errorOutput("ERROR: Invalid number of parameters.\n" + "Correct use: " + getInformation() + "\n");
                return;
            }
            if (File.Exists(parameters[0]))
            {
                m_model.loadMaze(parameters[0].ToLower(), parameters[1]);
                m_View.Output("Maze has been loaded successfuly from directory: \n| " + parameters[0] + " |\n");
            }
            else
            {
                m_View.errorOutput("ERROR: Unable to load maze, User given path invalid.\n");
            }
        }

        /// <summary>
        /// The string of the command
        /// </summary>
        /// <returns>String with the command details</returns>
        public override string getInformation()
        {
            return "loadMaze <file_path> <maze_name>";
        }

        /// <summary>
        /// Returns the name of the command
        /// </summary>
        /// <returns>String with the command name</returns>
        public override string GetName()
        {
            return "loadmaze";
        }
    }
}
