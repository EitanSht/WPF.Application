using Client.Presenter;
using MazeGenerators;
using Microsoft.Win32;
using Search;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for MazeWindow.xaml
    /// </summary>
    public partial class MazeWindow : Window, IView
    {
        public event View.func ViewChanged;

        public event View.func ViewStart;

        private Dictionary<string, Client.Presenter.ICommand> m_commands;
        private List<string> userInput;
        private string mCurrentCommandName;
        private WinMaze staticMaze;
        private WinMaze originalWinMaze;
        private bool mazeExists = false;
        private bool solutionExists = false;
        private bool mFinished = false;
        private double mWheelScale = 1;
        private string mazeTitle;
        private MediaPlayer mediaPlayer = new MediaPlayer();

        /// <summary>
        /// MazeWindow Constructor
        /// </summary>
        public MazeWindow()
        {
            IModel model = new MyModel();
            MyPresenter presenter = new MyPresenter(model, this);
            userInput = new List<string>();
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            mediaPlayer.Open(new Uri(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 10) + @"\Music\Rainbow6.mp3"));
            mediaPlayer.Play();
        }

        /// <summary>
        /// Generates a new maze with the user input
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void generateButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(levelsData.Text))
            {
                errorOutput("Please insert 'Levels' value");
                return;
            }
            if (string.IsNullOrEmpty(columnsData.Text))
            {
                errorOutput("Please insert 'Levels' value");
                return;
            }
            if (string.IsNullOrEmpty(rowsData.Text))
            {
                errorOutput("Please insert 'Rows' value");
                return;
            }
            if (string.IsNullOrEmpty(sizeData.Text))
            {
                errorOutput("Please insert 'Size' value");
                return;
            }

            int mLevels = Convert.ToInt32(levelsData.Text);
            int mColumns = Convert.ToInt32(columnsData.Text);
            int mRows = Convert.ToInt32(rowsData.Text);
            int mSize = Convert.ToInt32(sizeData.Text);
            mFinished = false;

            if ((mLevels < 1) || (mColumns < 6) || (mRows < 6) || (mSize < 10))
            {
                errorOutput("Incorrect maze input, Please insert:\n\nLevels - Above 1\nColumns - Above 6\nRows - Above 6");
                return;
            }

            if (((mSize * mColumns) > 380) || ((mSize * mRows) > 350))
            {
                errorOutput("Size of the maze is too large:\n\nPlease enter new dimetions\nor change the cell size");
                return;
            }
            currentFloor.Visibility = Visibility.Visible;
            upperFloorTxt.Visibility = Visibility.Visible;
            lowerFloorTxt.Visibility = Visibility.Visible;
            solveButton.Visibility = Visibility.Visible;
            solutionButton.Visibility = Visibility.Hidden;
            hideSolutionButton.Visibility = Visibility.Hidden;
            solveButton.IsEnabled = true;
            solutionExists = false;
            mazeTitle = "maze";
            mCurrentCommandName = "generate3dmaze";
            userInput.Clear();
            userInput.Add("maze");
            userInput.Add(mLevels.ToString());
            userInput.Add(mColumns.ToString());
            userInput.Add(mRows.ToString());
            userInput.Add(mSize.ToString());

            cnvs_main.Children.Clear();
            ViewChanged();
        }

        /// <summary>
        /// Displays the given floor of the maze
        /// </summary>
        /// <param name="winMaze">Current WinMaze</param>
        /// <param name="floor">Current floor to display</param>
        [STAThread]
        public void printMaze(WinMaze winMaze, int floor)
        {
            if ((winMaze.PosZ == 0) && (winMaze.PosX == (winMaze.getMaze().MyColumns - 1)) &&
                (winMaze.PosY == (winMaze.getMaze().MyRows - 1)) && !mFinished)
            {
                mFinished = true;
                FinishWindow finishWindow = new FinishWindow();
                finishWindow.Show();
            }

            staticMaze = winMaze;
            if (solutionExists)
            {
                solutionExists = false;
                winMaze = originalWinMaze;
                winMaze.clearSolution();
            }
            else
            {
                originalWinMaze = winMaze;
            }

            MazeBoard mazeBoard = new MazeBoard(winMaze, winMaze.PosZ, winMaze.CellSize, 0);
            cnvs_main.Children.Clear();
            cnvs_main.Children.Add(mazeBoard);
            Canvas.SetLeft(mazeBoard, 30);
            Canvas.SetTop(mazeBoard, 10);
            cnvs_second.Children.Clear();

            if ((winMaze.PosZ + 1) < winMaze.getMaze().MyHeight)
            {
                MazeBoard upMazeBoard = new MazeBoard(winMaze, floor, 10, 1);
                cnvs_second.Children.Add(upMazeBoard);
                Canvas.SetLeft(upMazeBoard, 0);
                Canvas.SetTop(upMazeBoard, 30);
            }

            if ((winMaze.PosZ - 1) >= 0)
            {
                MazeBoard downMazeBoard = new MazeBoard(winMaze, floor, 10, -1);
                cnvs_second.Children.Add(downMazeBoard);
                Canvas.SetLeft(downMazeBoard, 0);
                Canvas.SetTop(downMazeBoard, 200);
            }
            mazeExists = true;
            staticMaze = winMaze;
            Debug.WriteLine("exiting printmaze");
        }

        /// <summary>
        /// Returns all the initiated commands
        /// </summary>
        /// <returns>Dictionary type object containg all the commands</returns>
        public Dictionary<string, Presenter.ICommand> getAllCommands()
        {
            return m_commands;
        }

        /// <summary>
        /// Closes the current window
        /// </summary>
        public void exit()
        {
            this.Close();
        }

        /// <summary>
        /// Output in a standard layout - Message box
        /// </summary>
        /// <param name="errorOutput">String details</param>
        public void Output(string output)
        {
            MessageBox.Show(output, "Message");
        }

        /// <summary>
        /// Output in an error layout - Message box
        /// </summary>
        /// <param name="errorOutput">Error string</param>
        public void errorOutput(string errorOutput)
        {
            MessageBox.Show(errorOutput, "Error");
        }

        /// <summary>
        /// Returns the command title
        /// </summary>
        /// <returns>String - User command title</returns>
        public string getUserCommandName()
        {
            return mCurrentCommandName;
        }

        /// <summary>
        /// Returns the last user command
        /// </summary>
        /// <returns>String Array containing user command details</returns>
        public string[] getUserCommand()
        {
            return userInput.ToArray();
        }

        /// <summary>
        /// Event - View Start
        /// </summary>
        public void Start()
        {
            ViewStart();
        }

        /// <summary>
        /// Sets the commands
        /// </summary>
        /// <param name="commands">Dictionary containing the commands</param>
        public void setCommands(Dictionary<string, Presenter.ICommand> commands)
        {
            m_commands = commands;
        }

        /// <summary>
        /// Handles pressed keys - movement of the user
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var pressedKey = e.Key;
            if (!mazeExists)
                return;
            userInput.Clear();
            switch (pressedKey)
            {
                case Key.Left:
                    e.Handled = true;
                    userInput.Clear();
                    mCurrentCommandName = "left";
                    ViewChanged();
                    break;

                case Key.Right:
                    e.Handled = true;
                    userInput.Clear();
                    mCurrentCommandName = "right";
                    ViewChanged();
                    break;

                case Key.Down:
                    e.Handled = true;
                    userInput.Clear();
                    mCurrentCommandName = "forward";
                    ViewChanged();
                    break;

                case Key.Up:
                    e.Handled = true;
                    userInput.Clear();
                    mCurrentCommandName = "back";
                    ViewChanged();
                    break;

                case Key.PageDown:
                    e.Handled = true;
                    userInput.Clear();
                    mCurrentCommandName = "down";
                    ViewChanged();
                    break;

                case Key.PageUp:
                    e.Handled = true;
                    userInput.Clear();
                    mCurrentCommandName = "up";
                    ViewChanged();
                    break;
            }
            if (!e.Handled)
            {
                base.OnKeyDown(e);
            }
        }

        /// <summary>
        /// Launches the algorithm choice window
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void solveButtonClick(object sender, RoutedEventArgs e)
        {
            SolveChoiceWindow solveChoiceWindow = new SolveChoiceWindow();
            solveChoiceWindow.ShowDialog();
            string choice = solveChoiceWindow.getChoice();
            if (choice != "")
            {
                userInput.Clear();
                mCurrentCommandName = "solvemaze";
                userInput.Add(mazeTitle);
                userInput.Add(choice);
                solutionButton.Visibility = Visibility.Visible;
                solutionButton.IsEnabled = true;
            }
            solveButton.IsEnabled = false;


            ViewChanged();
        }

        /// <summary>
        /// Displays the solution
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void showSolutionClick(object sender, RoutedEventArgs e)
        {
            if (mazeExists)
            {
                userInput.Clear();
                mCurrentCommandName = "displaysolution";
                userInput.Add(mazeTitle);
                hideSolutionButton.Visibility = Visibility.Visible;
                solutionButton.Visibility = Visibility.Hidden;
                ViewChanged();
            }
        }

        /// <summary>
        /// Inserts a new solution from the Model to the WinMaze
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        public void injectSolution(List<string[]> solutionCoordinates)
        {
            List<string[]> tempList = solutionCoordinates;
            staticMaze.setSolutionCoordinates(tempList);
            //userInput.Clear();
            //WinMaze solutionMaze = staticMaze;
            //mCurrentCommandName = "display";
            //userInput.Add("maze");
            //ViewChanged();
        }

        /// <summary>
        /// Hides the maze solution
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void hideSolutionClick(object sender, RoutedEventArgs e)
        {
            solutionButton.Visibility = Visibility.Visible;
            hideSolutionButton.Visibility = Visibility.Hidden;
            staticMaze = originalWinMaze;
            solutionExists = true;
            printMaze(staticMaze, staticMaze.PosZ);
        }

        /// <summary>
        /// Clears the screen
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void newClicked(object sender, RoutedEventArgs e)
        {
            mazeTitle = "maze";
            solutionExists = false;
            mazeExists = false;
            cnvs_main.Children.Clear();
            cnvs_second.Children.Clear();
            currentFloor.Visibility = Visibility.Hidden;
            upperFloorTxt.Visibility = Visibility.Hidden;
            lowerFloorTxt.Visibility = Visibility.Hidden;
            solveButton.Visibility = Visibility.Hidden;
            solutionButton.Visibility = Visibility.Hidden;
            hideSolutionButton.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Allows the user to load a file
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void loadClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string filePath = "";
            string fileTitle = "";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                mCurrentCommandName = "loadmaze";
                fileTitle = openFileDialog.SafeFileName;
                mazeTitle = fileTitle.Remove(fileTitle.Length - 5);
                userInput.Clear();
                userInput.Add(filePath);
                userInput.Add(fileTitle);
                mazeExists = true;
                currentFloor.Visibility = Visibility.Visible;
                upperFloorTxt.Visibility = Visibility.Visible;
                lowerFloorTxt.Visibility = Visibility.Visible;
                solveButton.Visibility = Visibility.Visible;
                solutionButton.Visibility = Visibility.Hidden;
                hideSolutionButton.Visibility = Visibility.Hidden;
                solveButton.IsEnabled = true;
                solutionExists = false;
            }
            ViewChanged();
        }

        /// <summary>
        /// Launches the saving method
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void saveClicked(object sender, RoutedEventArgs e)
        {
            if (!mazeExists)
            {
                errorOutput("Please generate a maze");
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string filePath = "";
            string fileTitle = "";
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
                fileTitle = saveFileDialog.SafeFileName;
                filePath += ".maze";
                mazeTitle = fileTitle;
                mCurrentCommandName = "savemaze";
                userInput.Clear();
                userInput.Add(fileTitle);
                userInput.Add(filePath);
            }
            ViewChanged();
        }

        /// <summary>
        /// Launches the "Properties" window
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void propertiesClicked(object sender, RoutedEventArgs e)
        {
            PropertiesWindow propertiesWindow = new PropertiesWindow();
            propertiesWindow.Show();
        }

        /// <summary>
        /// Launches the "Exit" window
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void exitClicked(object sender, RoutedEventArgs e)
        {
            mCurrentCommandName = "exit";
            ViewChanged();
        }

        /// <summary>
        /// Launches the "Instructions" window
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void instructionsClicked(object sender, RoutedEventArgs e)
        {
            this.Hide();
            InstructionsWindow instructionsWindow = new InstructionsWindow();
            instructionsWindow.ShowDialog();
            this.Show();
        }

        /// <summary>
        /// Launches the "About Us" window
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void aboutClicked(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        /// <summary>
        /// Allows to use the mousewheel to zoom in/out: Primary mazes
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">Mouse data</param>
        private void cnvs_main_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            bool pressed = (Keyboard.Modifiers & ModifierKeys.Control) > 0;
            if (pressed)
            {
                double ratioScale;
                if (e.Delta > 0) // Enlarge
                {
                    mWheelScale *= 1.02;
                    ratioScale = cnvs_main.ActualWidth * mWheelScale;
                    if (ratioScale < 530)
                    {
                        mainMazeScale.ScaleX *= 1.02;
                        mainMazeScale.ScaleY *= 1.02;
                    }
                    else
                    {
                        mWheelScale /= 1.02;
                        MessageBox.Show("Maximum size achieved");
                    }
                }
                else // Shrink
                {
                    mWheelScale /= 1.02;
                    mainMazeScale.ScaleX /= 1.02;
                    mainMazeScale.ScaleY /= 1.02;
                }
            }
        }

        /// <summary>
        /// Primary mazes mouse button pressed implementation
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">Mouse data</param>
        private void cnvs_main_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this);
        }

        /// <summary>
        /// Allows to use the mousewheel to zoom in/out: Secondary mazes
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">Mouse data</param>
        private void cnvs_second_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            bool pressed = (Keyboard.Modifiers & ModifierKeys.Control) > 0;
            if (pressed)
            {
                if (e.Delta > 0) // Enlarge
                {
                    secondaryMazeScale.ScaleX *= 1.02;
                    secondaryMazeScale.ScaleY *= 1.02;
                }
                else // Shrink
                {
                    secondaryMazeScale.ScaleX /= 1.02;
                    secondaryMazeScale.ScaleY /= 1.02;
                }
            }
        }

        
    }
}