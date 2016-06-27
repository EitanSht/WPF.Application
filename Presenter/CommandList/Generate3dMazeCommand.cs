using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Wpf.ATP.Project.Model;
using Wpf.ATP.Project.View;

namespace Wpf.ATP.Project.Presenter
{
    /// <summary>
    /// Generate a 3d Maze - Generates a new maze with given user input
    /// </summary>
    /// 
    class Generate3dMazeCommand : ACommand
    {
        /// <summary>
        /// Generate3dMazeCommand command constructor
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="view">View</param>
        public Generate3dMazeCommand(IModel model, IView view)
            : base(model, view)
        {
            // Empty Constructor
        }

        /// <summary>
        /// Initates the generate3dMaze command with the given dimentions
        /// </summary>
        /// <param name="parameters">Levels, Columns and Rows of the maze - Dimentions</param>
        public override void DoCommand(params string[] parameters)
        {
            int levels, columns, rows, cellSize;
            if ((parameters.Length != 5) || !(int.TryParse(parameters[1], out levels)
                & int.TryParse(parameters[2], out columns) & int.TryParse(parameters[3], out rows) & int.TryParse(parameters[4], out cellSize)))
            {
                m_View.errorOutput("ERROR: Invalid number of parameters.\n");
                m_View.errorOutput("Correct use: \n" + getInformation() + "\n");
            }
            else if ((levels < 1 | levels > 255) | (columns < 6 | columns > 255) | (rows < 6 | rows > 255))
            {
                m_View.errorOutput("ERROR: Given maze dimentions are incorrect.\n");
            }
            else
            {
                m_model.generate3dMaze(parameters[0], levels, columns, rows, cellSize);
            }
        }

        /// <summary>
        /// The string of the command
        /// </summary>
        /// <returns>String with the command details</returns>
        public override string getInformation()
        {
            string str = "generate3dMaze <maze_name> <levels (1-255)> <columns (6-255)> <rows (6-255)>";
            return str;
        }

        /// <summary>
        /// Returns the name of the command
        /// </summary>
        /// <returns>String with the command name</returns>
        public override string GetName()
        {
            return "generate3dmaze";
        }
    }
}
