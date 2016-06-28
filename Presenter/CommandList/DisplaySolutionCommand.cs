using Search;
using Wpf.ATP.Project.Model;
using Wpf.ATP.Project.View;

namespace Wpf.ATP.Project.Presenter
{
    /// <summary>
    /// Display Solution - Displays the solution of the command
    /// </summary>
    internal class DisplaySolutionCommand : ACommand
    {
        /// <summary>
        /// DisplaySolutionCommand command constructor
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="view">View</param>
        public DisplaySolutionCommand(IModel model, IView view)
            : base(model, view)
        {
            // Empty Constructor
        }

        /// <summary>
        /// Initates the display solution command
        /// </summary>
        /// <param name="parameters">User input - maze name</param>
        public override void DoCommand(params string[] parameters)
        {
            if (parameters.Length != 1)
            {
                m_View.errorOutput("ERROR: Invalid number of parameters.\n");
                m_View.errorOutput("Correct use: " + getInformation() + "\n");
                return;
            }
            if (!m_model.mazeExists(parameters[0]))
            {
                m_View.errorOutput("ERROR: A maze named: | " + parameters[0] + " | does not exist.\n");
            }

            Solution solution = m_model.displaySolution(parameters[0]);
            if (null == solution)
            {
                m_View.errorOutput("ERROR: The solution does not exist for the maze named: " + parameters[0] + ".\n");
            }
            else // soultion exists
            {
                string solutionString = "The solution for the maze named | " + parameters[0] + " |:\n";
                solutionString += solution.GetSolutionPathByString() + "\n";
                solutionString += "Solution for the maze named: | " + parameters[0] + " | completed.";
                //m_View.Output(solutionString);
                m_View.injectSolution(solution.getSolutionCoordinates());
            }
        }

        /// <summary>
        /// The string of the command
        /// </summary>
        /// <returns>String with the command details</returns>
        public override string getInformation()
        {
            return "displaySolution <maze_name>";
        }

        /// <summary>
        /// Returns the name of the command
        /// </summary>
        /// <returns>String with the command name</returns>
        public override string GetName()
        {
            return "dislaysolution";
        }
    }
}