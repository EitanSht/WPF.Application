using Client.View;
using System.Collections.Generic;

namespace Client.Presenter
{
    /// <summary>
    /// Presenter part of the MVP module
    /// </summary>
    internal class MyPresenter
    {
        private IModel m_model;
        private IView m_view;
        private Dictionary<string, ICommand> m_commands;

        /// <summary>
        /// MyPresenter Constructor
        /// </summary>
        /// <param name="model">Current Model</param>
        /// <param name="view">Current View (WPF)</param>
        public MyPresenter(IModel model, IView view)
        {
            m_commands = new Dictionary<string, ICommand>();
            m_model = model;
            m_view = view;

            m_view.ViewStart += delegate () // View Start
            {
                createConsoleCommands();
                m_commands = m_view.getAllCommands();
            };

            m_view.ViewChanged += delegate () // View Changed
            {
                string[] userCommand = m_view.getUserCommand();
                string commandName = m_view.getUserCommandName();
                if (commandName == "exit")
                {
                    m_model.exit();
                }
                m_commands[commandName].DoCommand(userCommand);
                m_model.modelEvent();
            };

            m_model.ModelChanged += delegate () // Model Changed
            {
                string[] instructions = m_model.getInstructions();
                string[] userCommand = m_view.getUserCommand();
                string commandName = m_view.getUserCommandName();

                if (commandName == "exit")
                {
                    m_view.exit();
                }
                if (commandName == "generate3dmaze")
                {
                    WinMaze winMaze = m_model.getWinMaze(userCommand[0]);
                    m_view.printMaze(winMaze, winMaze.PosZ);
                }
                if (commandName == "displaysolution")
                {
                    WinMaze winMaze = m_model.getWinMaze("maze");
                    m_view.printMaze(winMaze, winMaze.PosZ);
                }
                if (instructions.Length > 0)
                {
                    if (instructions[0] == "display")
                    {
                        WinMaze winMaze = m_model.getWinMaze(instructions[1]);
                        if (null == winMaze)
                            m_view.errorOutput("winMaze in model presenter is = null");
                        else
                            m_view.printMaze(winMaze, winMaze.PosZ);
                    }
                }
            };

            m_view.Start();
        }

        /// <summary>
        /// Create Console Commands - Creates the available console commands
        /// </summary>
        private void createConsoleCommands()
        {
            m_commands.Add("generate3dmaze", new Generate3dMazeCommand(m_model, m_view));
            m_commands.Add("display", new DisplayMazeCommand(m_model, m_view));
            m_commands.Add("savemaze", new SaveMazeCommand(m_model, m_view));
            m_commands.Add("loadmaze", new LoadMazeCommand(m_model, m_view));
            m_commands.Add("solvemaze", new SolveMazeCommand(m_model, m_view));
            m_commands.Add("displaysolution", new DisplaySolutionCommand(m_model, m_view));
            m_commands.Add("left", new MoveLeftCommand(m_model, m_view));
            m_commands.Add("right", new MoveRightCommand(m_model, m_view));
            m_commands.Add("back", new MoveBackCommand(m_model, m_view));
            m_commands.Add("forward", new MoveForwardCommand(m_model, m_view));
            m_commands.Add("down", new MoveDownCommand(m_model, m_view));
            m_commands.Add("up", new MoveUpCommand(m_model, m_view));
            m_commands.Add("exit", new ExitCommand(m_model, m_view));
            m_view.setCommands(m_commands);
        }
    }
}