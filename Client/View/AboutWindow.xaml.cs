using System.Windows;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        /// <summary>
        /// AboutWindow Constructor
        /// </summary>
        public AboutWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        /// <summary>
        /// OK button pressed
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void okClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}