using Wpf.ATP.Project.Model;
using Wpf.ATP.Project.View;

namespace Wpf.ATP.Project.Presenter
{
    /// <summary>
    /// Abstract Class of the commands,
    /// is implemented by every command
    /// </summary>
    internal abstract class ACommand : ICommand
    {
        protected IModel m_model;
        protected IView m_View;

        /// <summary>
        /// Acommand constructor
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="view">View</param>
        public ACommand(IModel model, IView view)
        {
            m_model = model;
            m_View = view;
        }

        /// <summary>
        /// DoCommand - Abstract method that requests the information
        /// from the Model and represents the processed data in View.
        /// </summary>
        /// <param name="parameters">User Input - Varies from each command</param>
        public abstract void DoCommand(params string[] parameters);

        /// <summary>
        /// Get Name - Abstract method that returns the name of the command
        /// </summary>
        /// <returns>String of the command name</returns>
        public abstract string GetName();

        /// <summary>
        /// Get Information - Abstract method that returns the information of the command
        /// </summary>
        /// <returns>String with the command information</returns>
        public abstract string getInformation();
    }
}