using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MediaPlayer mediaPlayerMain = new MediaPlayer();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        /// <summary>
        /// MainWindow Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            mediaPlayerMain.Open(new Uri(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 10) + @"\Music\HeartBeat.mp3"));
            mediaPlayerMain.Play();
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE); // Hide
        }

        /// <summary>
        /// Begin Game button pressed
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void BeginButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            InstructionsWindow instructionsWindow = new InstructionsWindow(mediaPlayerMain, true);
            instructionsWindow.ShowDialog();
            this.Close();
        }

        /// <summary>
        /// Close Button Pressed
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}