using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for MazeCell.xaml
    /// </summary>
    public partial class MazeCell : UserControl
    {
        /// <summary>
        /// MazeCell Constructor
        /// </summary>
        /// <param name="mazeCellSize">Size of the cells</param>
        /// <param name="isSpecial">Whether it is a special cell</param>
        /// <param name="leftWall">Is there a wall on the left</param>
        /// <param name="rightWall">Is there a wall on the right</param>
        /// <param name="backWall">Is there a wall on the back</param>
        /// <param name="forwardWall">Is there a wall forward</param>
        /// <param name="isLeftWall">Is it the most left column</param>
        /// <param name="isUpWall">Is it the top row</param>
        public MazeCell(int mazeCellSize, int isSpecial, bool leftWall, bool rightWall, bool backWall, bool forwardWall, bool isLeftWall, bool isUpWall)
        {
            InitializeComponent();
            userControl.Width = mazeCellSize;

            lineLeftWall.Visibility = (isLeftWall) ? Visibility.Visible : Visibility.Hidden;
            lineRightWall.Visibility = (rightWall) ? Visibility.Visible : Visibility.Hidden;
            lineBackWall.Visibility = (isUpWall) ? Visibility.Visible : Visibility.Hidden;
            lineForwardWall.Visibility = (forwardWall) ? Visibility.Visible : Visibility.Hidden;
            if (isSpecial == 0) // Regualr
            {
                cellCanvas.Background = new SolidColorBrush(Colors.Transparent);
            }
            if (isSpecial == 1) // Block
                cellCanvas.Background = new SolidColorBrush(Colors.Yellow);
            if (isSpecial == 2)// Player
            {
                Uri resourceUri = new Uri("../Graphics/Player.png", UriKind.Relative);
                StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                var brush = new ImageBrush();
                brush.ImageSource = temp;
                cellCanvas.Background = brush;
            }
            if (isSpecial == 3) // Start
            {
                cellCanvas.Background = new SolidColorBrush(Colors.Blue);
            }
            if (isSpecial == 4) // End
            {
                cellCanvas.Background = new SolidColorBrush(Colors.Black);
            }
            if (isSpecial == 5) // Solution Cell
            {
                Uri resourceUri = new Uri("../Graphics/Red.png", UriKind.Relative);
                StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                var brush = new ImageBrush();
                brush.ImageSource = temp;
                cellCanvas.Background = brush;
            }
        }
    }
}