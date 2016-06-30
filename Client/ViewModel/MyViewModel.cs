using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.ViewModel
{
    /// <summary>
    /// The View-Model Part of the MVVM Design Pattern
    /// </summary>
    internal class MyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IModel m_model;

        /// <summary>
        /// MyViewModel Constructor
        /// </summary>
        /// <param name="model">Current Model</param>
        public MyViewModel(IModel model)
        {
            m_model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
            };
        }

        /// <summary>
        /// Initiates the generate maze method
        /// </summary>
        internal void generateMaze()
        {
            m_model.generate3dMaze();
        }

        /// <summary>
        /// Initiates the generate new screen method
        /// </summary>
        internal void newClicked()
        {
            m_model.newCanvas();
        }

        /// <summary>
        /// Initiates the exit method
        /// </summary>
        internal void exit()
        {
            m_model.exit();
        }

        /// <summary>
        /// Initiates the solving of the maze method - call for server
        /// </summary>
        internal void solveMazeClicked(string solvingChoice)
        {
            m_model.solveMaze(solvingChoice);
        }

        /// <summary>
        /// Initiates the show solution method
        /// </summary>
        internal void showSolution()
        {
            m_model.showSolution();
        }

        /// <summary>
        /// Initiates the hide solution method
        /// </summary>
        internal void hideSolution()
        {
            m_model.hideSolution();
        }

        /// <summary>
        /// Initiates the load method
        /// </summary>
        internal void loadClicked()
        {
            m_model.loadClicked();
        }

        /// <summary>
        /// Initiates the save method
        /// </summary>
        internal void saveClicked()
        {
            m_model.saveClicked();
        }

        /// <summary>
        /// Initiates movement in the maze
        /// </summary>
        /// <param name="sender">No Implementation</param>
        /// <param name="e">Pressed key</param>
        public void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var key = e.Key;

            switch (key)
            {
                case Key.Down:
                    e.Handled = true;
                    m_model.moveForward();
                    break;

                case Key.Up:
                    e.Handled = true;
                    m_model.moveBack();
                    break;

                case Key.Right:
                    e.Handled = true;
                    m_model.moveRight();
                    break;

                case Key.Left:
                    e.Handled = true;
                    m_model.moveLeft();
                    break;

                case Key.PageDown:
                    e.Handled = true;
                    m_model.moveDown();
                    break;

                case Key.PageUp:
                    e.Handled = true;
                    m_model.moveUp();
                    break;
            }
        }

        #region Properties

        public string LevelData
        {
            get { return m_model.LevelData; }
            set { m_model.LevelData = value; }
        }

        public string ColumnData
        {
            get { return m_model.ColumnData; }
            set { m_model.ColumnData = value; }
        }

        public string RowData
        {
            get { return m_model.RowData; }
            set { m_model.RowData = value; }
        }

        public string CellSizeData
        {
            get { return m_model.CellSizeData; }
            set { m_model.CellSizeData = value; }
        }

        public Canvas CurrentMazeCanvas
        {
            get { return m_model.CurrentMazeCanvas; }
            set { m_model.CurrentMazeCanvas = value; }
        }

        public Canvas SecondaryMazeCanvas
        {
            get { return m_model.SecondaryMazeCanvas; }
            set { m_model.SecondaryMazeCanvas = value; }
        }

        public string Output
        {
            get { return m_model.Output; }
            set { m_model.Output = value; }
        }

        public string ErrorOutput
        {
            get { return m_model.ErrorOutput; }
            set { m_model.ErrorOutput = value; }
        }

        #endregion Properties
    }
}