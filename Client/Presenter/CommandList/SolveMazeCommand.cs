using Client.View;

namespace Client.Presenter
{
    /// <summary>
    /// Solve Maze - Solves the given maze with a given
    /// solving algorithm - BFS/DFS
    /// </summary>
    internal class SolveMazeCommand : ACommand
    {
        /// <summary>
        /// SolveMazeCommand command constructor
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="view">View</param>
        public SolveMazeCommand(IModel model, IView view)
            : base(model, view)
        {
            // Empty Constructor
        }

        /// <summary>
        /// Initates the solveMaze command
        /// </summary>
        /// <param name="parameters">Maze name, Solving algorithm - BFS/DFS</param>
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
                m_model.solveMaze(parameters[0], parameters[1]);
            }
        }

        /// <summary>
        /// The string of the command
        /// </summary>
        /// <returns>String with the command details</returns>
        public override string getInformation()
        {
            return "solveMaze <maze_name> <BFS/DFS>";
        }

        /// <summary>
        /// Returns the name of the command
        /// </summary>
        /// <returns>String with the command name</returns>
        public override string GetName()
        {
            return "solvemaze";
        }
    }
}