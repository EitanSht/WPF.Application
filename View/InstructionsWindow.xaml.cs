using System.Windows;
using System.Windows.Media;

namespace Wpf.ATP.Project.View
{
    /// <summary>
    /// Interaction logic for InstructionsWindow.xaml
    /// </summary>
    public partial class InstructionsWindow : Window
    {
        private MediaPlayer mediaPlayerMain = null;

        public InstructionsWindow(MediaPlayer mediaPlayerMain)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.mediaPlayerMain = mediaPlayerMain;
        }

        public InstructionsWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void okButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MazeWindow mazeWindow = new MazeWindow();
            mediaPlayerMain?.Stop();
            mazeWindow.ShowDialog();
            this.Close();
        }
    }
}