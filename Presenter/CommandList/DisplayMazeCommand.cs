using Wpf.ATP.Project.Model;
using Wpf.ATP.Project.View;
using MazeGenerators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.ATP.Project.Presenter
{
    /// <summary>
    /// Display Maze - Displays the loaded or created maze
    /// </summary>
    class DisplayMazeCommand : ACommand
    {
        /// <summary>
        /// DisplayMazeCommand command constructor
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="view">View</param>
        public DisplayMazeCommand(IModel model, IView view)
            : base(model, view)
        {
            // Empty Constructor
        }

        /// <summary>
        /// Initiates the display command
        /// </summary>
        /// <param name="parameters">User input - maze name</param>
        public override void DoCommand(params string[] parameters)
        {
            if (parameters.Length != 2)
            {
                m_View.errorOutput("ERROR: Invalid number of parameters.\n");
                m_View.errorOutput("Correct use: " + getInformation() + "\n");
                return;
            }
            else
            {
                WinMaze receivedMaze;
                try
                {
                    receivedMaze = m_model.getWinMaze(parameters[0]);
                }
                catch (Exception)
                {
                    m_View.errorOutput("ERROR: Maze does not exist.\n");
                    return;
                }
                m_View.printMaze(receivedMaze, Convert.ToInt32(parameters[1]));
            }
        }

        /// <summary>
        /// The string of the command
        /// </summary>
        /// <returns>String with the command details</returns>
        public override string getInformation()
        {
            return "display <maze_name>";
        }

        /// <summary>
        /// Returns the name of the command
        /// </summary>
        /// <returns>String with the command name</returns>
        public override string GetName()
        {
            return "dislay";
        }
    }
}
