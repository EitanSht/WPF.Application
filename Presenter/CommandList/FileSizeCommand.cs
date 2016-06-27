using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.ATP.Project.Model;
using Wpf.ATP.Project.View;

namespace Wpf.ATP.Project.Presenter
{
    /// <summary>
    /// File Size - Returns the size of the file in bytes
    /// </summary>
    class FileSizeCommand : ACommand
    {
        /// <summary>
        /// FileSizeCommand command constructor
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="view">View</param>
        public FileSizeCommand(IModel model, IView view)
            : base(model, view)
        {
            // Empty Constructor
        }

        /// <summary>
        /// Initates the command that returns the size of the file
        /// </summary>
        /// <param name="parameters">File path</param>
        public override void DoCommand(params string[] parameters)
        {
            if (parameters.Length != 1)
            {
                m_View.errorOutput("ERROR: Invalid number of parameters.\n");
                m_View.errorOutput("Correct use: " + getInformation() + "\n");
                return;
            }
            if (!File.Exists(parameters[0]))
            {
                m_View.errorOutput("ERROR: The path input is not available or incorrect.\n");
                return;
            }
            long fileSize = m_model.fileSize(parameters[0]);
            m_View.Output("File : | " + parameters[0] + " |");
            m_View.Output("Size : | " + fileSize.ToString() + " bytes |\n");
        }

        /// <summary>
        /// The string of the command
        /// </summary>
        /// <returns>String with the command details</returns>
        public override string getInformation()
        {
            return "fileSize <file_path>";
        }

        /// <summary>
        /// Returns the name of the command
        /// </summary>
        /// <returns>String with the command name</returns>
        public override string GetName()
        {
            return "filesize";
        }
    }
}
