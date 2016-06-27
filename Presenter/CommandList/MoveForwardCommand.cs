using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.ATP.Project.Model;
using Wpf.ATP.Project.View;

namespace Wpf.ATP.Project.Presenter
{
    /// <summary>
    /// Move Forward - Moves the playes forward in the maze
    /// </summary>
    class MoveForwardCommand : ACommand
    {
        /// <summary>
        /// MoveForwardCommand Command constructor
        /// </summary>
        /// <param name="model">Current Model</param>
        /// <param name="view">Current View</param>
        public MoveForwardCommand(IModel model, IView view)
            : base(model, view)
        {
            // Empty Constructor
        }

        /// <summary>
        /// Performs the Move Forward command
        /// </summary>
        /// <param name="parameters"></param>
        public override void DoCommand(params string[] parameters)
        {
            m_model.moveForward(m_model.getWinMaze("maze"));
        }

        /// <summary>
        /// The string of the command
        /// </summary>
        /// <returns>String with the command details</returns>
        public override string getInformation()
        {
            return "moveForward <maze_name>";
        }

        /// <summary>
        /// Returns the name of the command
        /// </summary>
        /// <returns>String with the command name</returns>
        public override string GetName()
        {
            return "forward";
        }
    }
}
