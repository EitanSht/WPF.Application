using System.Windows;

namespace Wpf.ATP.Project.View
{
    /// <summary>
    /// Interaction logic for PropertiesWindow.xaml
    /// </summary>
    public partial class PropertiesWindow : Window
    {
        private int workerThreads;
        private int completionPortThreads;
        private string solvingAlgorithm;

        public class TextboxText
        {
            public string textdata { get; set; }
        }

        public int WorkerThreads
        {
            get { return workerThreads; }
            set
            {
                workerThreads = value;
            }
        }

        public int CompletionPortThreads
        {
            get { return completionPortThreads; }
            set
            {
                completionPortThreads = value;
            }
        }

        public string SolvingAlgorithm
        {
            get { return solvingAlgorithm; }
            set
            {
                solvingAlgorithm = value;
            }
        }

        public PropertiesWindow()
        {
            WorkerThreads = Properties.Settings.Default.WorkerThreads;
            CompletionPortThreads = Properties.Settings.Default.CompletionPortThreads;
            SolvingAlgorithm = Properties.Settings.Default.SolvingAlgorithm;

            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            workerSlot.DataContext = new TextboxText() { textdata = WorkerThreads.ToString() };
            threadSlot.DataContext = new TextboxText() { textdata = CompletionPortThreads.ToString() };
            solvingSlot.DataContext = new TextboxText() { textdata = SolvingAlgorithm };
        }

        private void okClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}