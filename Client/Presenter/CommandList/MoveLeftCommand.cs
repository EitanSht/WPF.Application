using Client.View;

namespace Client.Presenter
{
    /// <summary>
    /// Move left - Moves the playes left in the maze
    /// </summary>
    internal class MoveLeftCommand : ACommand
    {
        /// <summary>
        /// MoveLeftCommand Command constructor
        /// </summary>
        /// <param name="model">Current Model</param>
        /// <param name="view">Current View</param>
        public MoveLeftCommand(IModel model, IView view)
            : base(model, view)
        {
            // Empty Constructor
        }

        /// <summary>
        /// Performs the Move Left command
        /// </summary>
        /// <param name="parameters"></param>
        public override void DoCommand(params string[] parameters)
        {
            m_model.moveLeft(m_model.getWinMaze("maze"));
        }

        /// <summary>
        /// The string of the command
        /// </summary>
        /// <returns>String with the command details</returns>
        public override string getInformation()
        {
            return "moveLeft <maze_name>";
        }

        /// <summary>
        /// Returns the name of the command
        /// </summary>
        /// <returns>String with the command name</returns>
        public override string GetName()
        {
            return "left";
        }
    }
}