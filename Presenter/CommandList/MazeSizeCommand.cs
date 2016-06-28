using Wpf.ATP.Project.Model;
using Wpf.ATP.Project.View;

namespace Wpf.ATP.Project.Presenter
{
    /// <summary>
    /// Maze Size - Returns the size of the maze object in the memory
    /// </summary>
    internal class MazeSizeCommand : ACommand
    {
        /// <summary>
        /// MazeSizeCommand command constructor
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="view">View</param>
        public MazeSizeCommand(IModel model, IView view)
            : base(model, view)
        {
            // Empty Constructor
        }

        /// <summary>
        /// Initates the mazeSize command
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
            int mazeSize = m_model.mazeSize(parameters[0]);
            if (mazeSize != -1)
            {
                m_View.Output("Maze : | " + parameters[0] + " |\n" + "Size : | " + mazeSize.ToString() + " bytes |\n");
            }
            else // Failed
            {
                m_View.errorOutput("ERROR: Unable to load the maze by its name.\n");
            }
        }

        /// <summary>
        /// The string of the command
        /// </summary>
        /// <returns>String with the command details</returns>
        public override string getInformation()
        {
            return "mazeSize <maze_name>";
        }

        /// <summary>
        /// Returns the name of the command
        /// </summary>
        /// <returns>String with the command name</returns>
        public override string GetName()
        {
            return "mazesize";
        }
    }
}