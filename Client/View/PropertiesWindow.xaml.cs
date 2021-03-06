﻿using System.Windows;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for PropertiesWindow.xaml
    /// </summary>
    public partial class PropertiesWindow : Window
    {
        private int workerThreads;
        private int completionPortThreads;
        private string solvingAlgorithm;
        private string ipAddress;
        private int port;

        /// <summary>
        /// Displays the window
        /// </summary>
        public PropertiesWindow()
        {
            WorkerThreads = Properties.Settings.Default.WorkerThreads;
            CompletionPortThreads = Properties.Settings.Default.CompletionPortThreads;
            SolvingAlgorithm = Properties.Settings.Default.SolvingAlgorithm;
            IPaddress = Properties.Settings.Default.IP;
            Port = Properties.Settings.Default.Port;

            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            workerSlot.DataContext = new TextboxText() { textdata = WorkerThreads.ToString() };
            threadSlot.DataContext = new TextboxText() { textdata = CompletionPortThreads.ToString() };
            solvingSlot.DataContext = new TextboxText() { textdata = SolvingAlgorithm };
            ipAddressSlot.DataContext = new TextboxText() { textdata = SolvingAlgorithm };
            portSlot.DataContext = new TextboxText() { textdata = SolvingAlgorithm };
        }

        /// <summary>
        /// Method to handle the OK button clicking
        /// </summary>
        /// <param name="sender">No implementation</param>
        /// <param name="e">Mouse</param>
        private void okClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region Properties

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

        public string IPaddress
        {
            get { return ipAddress; }
            set
            {
                ipAddress = value;
            }
        }

        public int Port
        {
            get { return port; }
            set
            {
                port = value;
            }
        }

        #endregion Properties
    }
}