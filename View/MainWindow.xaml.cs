using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.ATP.Project.Model;
using Wpf.ATP.Project.Presenter;
using Wpf.ATP.Project.View;

namespace Wpf.ATP.Project.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        /// <summary>
        /// MainWindow Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            mediaPlayer.Open(new Uri(Directory.GetCurrentDirectory() + @"\HeartBeat.mp3"));
            mediaPlayer.Play();
        }

        /// <summary>
        /// Begin Game button pressed
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">No implementation</param>
        private void BeginButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            InstructionsWindow instructionsWindow = new InstructionsWindow();
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
