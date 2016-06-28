using System;
using System.IO;
using Wpf.ATP.Project.Model;
using Wpf.ATP.Project.View;

namespace Wpf.ATP.Project.Presenter
{
    /// <summary>
    /// Directory Command - Returns file names & paths
    /// with a given directory
    /// </summary>
    internal class DirCommand : ACommand
    {
        /// <summary>
        /// DirDirCommand command constructor
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="view">View</param>
        public DirCommand(IModel model, IView view)
            : base(model, view)
        {
            // Empty Constructor
        }

        /// <summary>
        /// Requests the user (Model) for the file path
        /// Prints te information on screen (View)
        /// </summary>
        /// <param name="parameters">User input - path</param>
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
                try
                {
                    if (Directory.Exists(parameters[0].ToLower()))
                    {
                        string[] dirData = m_model.getDir(parameters[0].ToLower());
                        foreach (string dirLine in dirData)
                        {
                            m_View.Output(dirLine + " ");
                        }
                        m_View.Output("\n");
                    }
                    else
                    {
                        m_View.errorOutput("ERROR: Incorrect file path: " + parameters[0] + ".\n");
                    }
                }
                catch (Exception)
                {
                    m_View.errorOutput("ERROR: Invalid number of parameters.\n");
                    m_View.errorOutput("Correct use: " + getInformation() + "\n");
                }
            }
        }

        /// <summary>
        /// The string of the command
        /// </summary>
        /// <returns>String with the command details</returns>
        public override string getInformation()
        {
            return "dir <file_path>";
        }

        /// <summary>
        /// Returns the name of the command
        /// </summary>
        /// <returns>String with the command name</returns>
        public override string GetName()
        {
            return "dir";
        }
    }
}