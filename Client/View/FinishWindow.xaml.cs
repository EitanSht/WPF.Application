using System.Windows;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for FinishWindow.xaml
    /// </summary>
    public partial class FinishWindow : Window
    {
        /// <summary>
        /// FinishWindow Constructor
        /// </summary>
        public FinishWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        /// <summary>
        /// OK button pressed
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void finishedButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}