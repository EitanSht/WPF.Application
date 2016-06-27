using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf.ATP.Project.Model;
using Wpf.ATP.Project.View;

namespace Wpf.ATP.Project.Presenter
{
    /// <summary>
    /// Insert Solution - Inserts a solution to a given maze
    /// </summary>
    class InsertSolutionCommand : ACommand
    {
        /// <summary>
        /// InsertSolutionCommand command constructor
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="view">View</param>
        public InsertSolutionCommand(IModel model, IView view)
            : base(model, view)
        {
            // Empty Constructor
        }

        /// <summary>
        /// Initates the Insert Solution command
        /// </summary>
        /// <param name="parameters">Maze name</param>
        public override void DoCommand(params string[] parameters)
        {
            if (parameters.Length != 1)
            {
                m_View.errorOutput("ERROR: Invalid number of parameters.\n");
                m_View.errorOutput("Correct use: " + getInformation() + "\n");
                return;
            }
            else
            {
                m_model.insertSolution(parameters[0]);
            }
        }

        /// <summary>
        /// The string of the command
        /// </summary>
        /// <returns>String with the command details</returns>
        public override string getInformation()
        {
            return "insertSolution <maze_name>";
        }

        /// <summary>
        /// Returns the name of the command
        /// </summary>
        /// <returns>String with the command name</returns>
        public override string GetName()
        {
            return "insertSolution";
        }
    }
}
