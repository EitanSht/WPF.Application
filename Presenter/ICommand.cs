using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.ATP.Project.Presenter
{
    /// <summary>
    /// Interface that will be implemented in order to use commands
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// DoCommand - To be implemented method that requests the information 
        /// from the Model and represents the processed data in View.
        /// </summary>
        /// <param name="parameters">User Input - Varies from each command</param>
        void DoCommand(params string[] parameters);

        /// <summary>
        /// Get Name - To be implemented method that returns the name of the command
        /// </summary>
        /// <returns>String of the command name</returns>
        string GetName();

        /// <summary>
        /// Get Information - To be implemented method that returns the 
        /// information of the command
        /// </summary>
        /// <returns>String with the command information</returns>
        string getInformation();
    }
}
