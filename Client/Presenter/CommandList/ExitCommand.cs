using Client.View;

namespace Client.Presenter
{
    /// <summary>
    /// Exit - Quits the program safely
    /// </summary>
    internal class ExitCommand : ACommand
    {
        /// <summary>
        /// ExitCommand command constructor
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="view">View</param>
        public ExitCommand(IModel model, IView view)
            : base(model, view)
        {
            // Empty Constructor
        }

        /// <summary>
        /// Initates the Exit command
        /// </summary>
        /// <param name="parameters">No input</param>
        public override void DoCommand(params string[] parameters)
        {
            //m_model.Stop(); // No need to use it
            m_model.exit();
        }

        /// <summary>
        /// The string of the command
        /// </summary>
        /// <returns>String with the command details</returns>
        public override string getInformation()
        {
            return "exit";
        }

        /// <summary>
        /// Returns the name of the command
        /// </summary>
        /// <returns>String with the command name</returns>
        public override string GetName()
        {
            return "exit";
        }
    }
}