using System.Windows;
using System.Windows.Media;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for InstructionsWindow.xaml
    /// </summary>
    public partial class InstructionsWindow : Window
    {
        private MediaPlayer mediaPlayerMain = null;
        private bool mFromMain = false;

        /// <summary>
        /// InstructionsWindow advanced constructor
        /// </summary>
        /// <param name="mediaPlayerMain">Media player</param>
        /// <param name="fromMain">Did it come from main window</param>
        public InstructionsWindow(MediaPlayer mediaPlayerMain, bool fromMain)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            mFromMain = fromMain;
            if (mFromMain)
                this.mediaPlayerMain = mediaPlayerMain;
        }

        /// <summary>
        /// InstructionsWindow default constructor
        /// </summary>
        public InstructionsWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        /// <summary>
        /// Handles the OK button click
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">Mouse</param>
        private void okButtonClick(object sender, RoutedEventArgs e)
        {
            if (mFromMain)
            {
                this.Hide();
                MazeWindow mazeWindow = new MazeWindow();
                mediaPlayerMain?.Stop();
                mazeWindow.ShowDialog();
            }
            this.Close();
        }
    }
}