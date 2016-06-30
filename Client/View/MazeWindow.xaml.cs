using Client.ViewModel;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for MazeWindow.xaml
    /// </summary>
    public partial class MazeWindow : Window, IView
    {
        private double mWheelScale = 1;
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private MyViewModel m_vm;

        /// <summary>
        /// MazeWindow Constructor
        /// </summary>
        public MazeWindow()
        {
            IModel model = new MyModel();
            InitializeComponent();
            initiateMusic();
            m_vm = new MyViewModel(model);
            DataContext = m_vm;
        }

        private void initiateMusic()
        {
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
            m_vm.generateMaze();

            currentFloor.Visibility = Visibility.Visible;
            upperFloorTxt.Visibility = Visibility.Visible;
            lowerFloorTxt.Visibility = Visibility.Visible;
            solveButton.Visibility = Visibility.Visible;
            solutionButton.Visibility = Visibility.Hidden;
            hideSolutionButton.Visibility = Visibility.Hidden;
            solveButton.IsEnabled = true;
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
        public void Output()
        {
            MessageBox.Show(m_vm.Output, "Message");
        }

        /// <summary>
        /// Output in an error layout - Message box
        /// </summary>
        /// <param name="errorOutput">Error string</param>
        public void errorOutput()
        {
            MessageBox.Show(m_vm.ErrorOutput, "Error Message");
        }

        /// <summary>
        /// Handles pressed keys - movement of the user
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            m_vm.Window_KeyDown(sender, e);
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
            m_vm.solveMazeClicked(choice);
            if (choice != "")
            {
                solutionButton.Visibility = Visibility.Visible;
                solutionButton.IsEnabled = true;
            }
            solveButton.IsEnabled = false;
        }

        /// <summary>
        /// Displays the solution
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void showSolutionClick(object sender, RoutedEventArgs e)
        {
            m_vm.showSolution();
            hideSolutionButton.Visibility = Visibility.Visible;
            solutionButton.Visibility = Visibility.Hidden;
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
            m_vm.hideSolution();
        }

        /// <summary>
        /// Allows the user to load a file
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void loadClicked(object sender, RoutedEventArgs e)
        {
            m_vm.loadClicked();
            currentFloor.Visibility = Visibility.Visible;
            upperFloorTxt.Visibility = Visibility.Visible;
            lowerFloorTxt.Visibility = Visibility.Visible;
            solveButton.Visibility = Visibility.Visible;
            solutionButton.Visibility = Visibility.Hidden;
            hideSolutionButton.Visibility = Visibility.Hidden;
            solveButton.IsEnabled = true;
        }

        /// <summary>
        /// Launches the saving method
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void saveClicked(object sender, RoutedEventArgs e)
        {
            m_vm.saveClicked();
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
        /// Clears the screen
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void newClicked(object sender, RoutedEventArgs e)
        {
            m_vm.newClicked();
            currentFloor.Visibility = Visibility.Hidden;
            upperFloorTxt.Visibility = Visibility.Hidden;
            lowerFloorTxt.Visibility = Visibility.Hidden;
            solveButton.Visibility = Visibility.Hidden;
            solutionButton.Visibility = Visibility.Hidden;
            hideSolutionButton.Visibility = Visibility.Hidden;
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
            m_vm.exit();
            this.Close();
        }

        /// <summary>
        /// Launches the "Instructions" window
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void instructionsClicked(object sender, RoutedEventArgs e)
        {
            this.Hide();
            InstructionsWindow instructionsWindow = new InstructionsWindow(null, false);
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
    }
}