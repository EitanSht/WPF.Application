using System.Windows;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for SolveChoiceWindow.xaml
    /// </summary>
    public partial class SolveChoiceWindow : Window
    {
        private string mChoice = Properties.Settings.Default.SolvingAlgorithm;

        /// <summary>
        /// Window that allowes to chose the solving algorithm
        /// </summary>
        public SolveChoiceWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        /// <summary>
        /// Choses the BFS as the chosen algorithm
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void bfsButtonClick(object sender, RoutedEventArgs e)
        {
            mChoice = "BFS";
            this.Close();
        }

        /// <summary>
        /// Choses the DFS as the chosen algorithm
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void dfsButtonClick(object sender, RoutedEventArgs e)
        {
            mChoice = "DFS";
            this.Close();
        }

        /// <summary>
        /// Allows to cancel
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void cancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Returns the users choice
        /// </summary>
        /// <returns>String - Choise: BFS/DFS</returns>
        public string getChoice()
        {
            return mChoice;
        }
    }
}