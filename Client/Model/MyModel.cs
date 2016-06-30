using Client.View;
using Compression;
using MazeGenerators;
using Microsoft.Win32;
using Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Client
{
    /// <summary>
    /// Model part of the MVVM Design Pattern
    /// </summary>
    internal class MyModel : IModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Members & Variables

        private Dictionary<string, Maze3d> m_mazesDictionary;
        private Dictionary<string, Solution> m_solutionsDictionary;
        private Dictionary<string, WinMaze> m_winMazesDictionary;
        private WinMaze m_currentWinMaze;
        private string directoryPath = Directory.GetCurrentDirectory() + ("\\");
        private List<IStoppable> m_stoppingList = new List<IStoppable>();
        private List<string> m_instructions;
        private Solution mCurrentSolution = new Solution();
        private bool isSolutionExists = false;
        private bool mFinished = false;
        private WinMaze staticMaze;
        private WinMaze originalWinMaze;
        private bool isMazeExists = false;
        private bool solExists = false;
        private string currentMazeName = "maze";

        #endregion Members & Variables

        /// <summary>
        /// MyModel constructor
        /// </summary>
        public MyModel()
        {
            ActivateThreadPool();
            m_mazesDictionary = new Dictionary<string, Maze3d>();
            m_solutionsDictionary = new Dictionary<string, Solution>();
            m_winMazesDictionary = new Dictionary<string, WinMaze>();
            m_instructions = new List<string>();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Activates the thread pool
        /// </summary>
        private void ActivateThreadPool()
        {
            int workerThreads = Properties.Settings.Default.WorkerThreads;
            int completionPortThreads = Properties.Settings.Default.CompletionPortThreads;
            ThreadPool.SetMaxThreads(workerThreads, completionPortThreads);
        }

        /// <summary>
        /// Generate a 3d Maze - Generates a new maze with given user input
        /// </summary>
        /// <param name="name">Maze Name</param>
        /// <param name="levels">How many levels to the maze</param>
        /// <param name="columns">How many columns to the maze</param>
        /// <param name="rows">How many rows to the maze</param>
        [STAThread]
        public void generate3dMaze()
        {
            string mazeName = currentMazeName;
            if (string.IsNullOrEmpty(LevelData))
            {
                ErrorOutput = ("Please insert 'Levels' value");
                MessageBox.Show(ErrorOutput, "Error");
                return;
            }
            if (string.IsNullOrEmpty(ColumnData))
            {
                ErrorOutput = ("Please insert 'Column' value");
                MessageBox.Show(ErrorOutput, "Error");
                return;
            }
            if (string.IsNullOrEmpty(RowData))
            {
                ErrorOutput = ("Please insert 'Rows' value");
                MessageBox.Show(ErrorOutput, "Error");
                return;
            }
            if (string.IsNullOrEmpty(CellSizeData))
            {
                ErrorOutput = ("Please insert 'Size' value");
                MessageBox.Show(ErrorOutput, "Error");
                return;
            }
            if ((Int32.Parse(LevelData) < 1) || (Int32.Parse(ColumnData) < 6) || (Int32.Parse(RowData) < 6) || (Int32.Parse(CellSizeData) < 10))
            {
                ErrorOutput = ("Incorrect maze input, Please insert:\n\nLevels - Above 1\nColumns - Above 6\nRows - Above 6");
                MessageBox.Show(ErrorOutput, "Error");
                return;
            }

            if (((Int32.Parse(CellSizeData) * Int32.Parse(ColumnData)) > 380) || ((Int32.Parse(CellSizeData) * Int32.Parse(RowData)) > 350))
            {
                ErrorOutput = ("Size of the maze is too large:\n\nPlease enter new dimetions\nor change the cell size");
                MessageBox.Show(ErrorOutput, "Error");
                return;
            }
            int[] mazeDimentions = { Int32.Parse(LevelData), Int32.Parse(ColumnData), Int32.Parse(RowData) };
            int cellSize = Int32.Parse(CellSizeData);

            var resetEvent = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
            {
                threadGenerateMaze(mazeDimentions, mazeName, cellSize);
                resetEvent.Set();
            }));
            resetEvent.WaitOne();

            WinMaze winMaze = getWinMaze(mazeName);
            printMaze(winMaze, winMaze.PosZ);
        }

        /// <summary>
        /// Displays the current state of the maze in view
        /// </summary>
        /// <param name="winMaze">WinMaze object</param>
        /// <param name="floor">Current floor to displays</param>
        private void printMaze(WinMaze winMaze, int floor)
        {
            CurrentMazeCanvas = new Canvas();
            SecondaryMazeCanvas = new Canvas();

            if ((winMaze.PosZ == 0) && (winMaze.PosX == (winMaze.getMaze().MyColumns - 1)) &&
                (winMaze.PosY == (winMaze.getMaze().MyRows - 1)) && !mFinished)
            {
                mFinished = true;
                FinishWindow finishWindow = new FinishWindow();
                finishWindow.Show();
            }

            staticMaze = winMaze;
            if (solExists)
            {
                solExists = false;
                winMaze = originalWinMaze;
                winMaze.clearSolution();
            }
            else
            {
                originalWinMaze = winMaze;
            }

            MazeBoard mazeBoard = new MazeBoard(winMaze, winMaze.PosZ, winMaze.CellSize, 0);
            CurrentMazeCanvas.Children.Add(mazeBoard);
            Canvas.SetLeft(mazeBoard, 30);
            Canvas.SetTop(mazeBoard, 10);

            if ((winMaze.PosZ + 1) < winMaze.getMaze().MyHeight) // Upper level display
            {
                MazeBoard upMazeBoard = new MazeBoard(winMaze, floor, 10, 1);
                SecondaryMazeCanvas.Children.Add(upMazeBoard);
                Canvas.SetLeft(upMazeBoard, 0);
                Canvas.SetTop(upMazeBoard, 30);
            }

            if ((winMaze.PosZ - 1) >= 0)
            {
                MazeBoard downMazeBoard = new MazeBoard(winMaze, floor, 10, -1); // Lower level display
                SecondaryMazeCanvas.Children.Add(downMazeBoard);
                Canvas.SetLeft(downMazeBoard, 0);
                Canvas.SetTop(downMazeBoard, 200);
            }
            isMazeExists = true;
            staticMaze = winMaze;
        }

        /// <summary>
        /// Thread of maze generation to be implemented as a thread
        /// </summary>
        /// <param name="mazeDimentions">Maze dimentions</param>
        /// <param name="mazeName">Maze name</param>
        [STAThread]
        private void threadGenerateMaze(int[] mazeDimentions, string mazeName, int cellSize)
        {
            IMazeGenerator maze3dGenerator = new MyMaze3dGenerator();
            m_stoppingList.Add(maze3dGenerator);
            Maze3d currentMaze = (Maze3d)maze3dGenerator.generate(mazeDimentions);
            WinMaze winMaze = new WinMaze(currentMaze, mazeName, cellSize);
            m_winMazesDictionary[mazeName] = winMaze;
            m_currentWinMaze = winMaze;
            m_stoppingList.Remove(maze3dGenerator);
            m_solutionsDictionary.Remove("maze");
            add3dMaze("maze", currentMaze);
            originalWinMaze = winMaze;
            isSolutionExists = false;
        }

        /// <summary>
        /// Returns an existing maze by its name
        /// </summary>
        /// <param name="mazeName">Maze name</param>
        /// <returns>Dictionary type object representing a maze</returns>
        public Dictionary<int, Maze2d> getMaze(string mazeName)
        {
            return getMazeByName(mazeName).getFullMaze();
        }

        /// <summary>
        /// Save Maze - Creates a compression of the maze and
        /// saves the file on the disk with a given file path
        /// </summary>
        /// <param name="mazeName">Maze name</param>
        /// <param name="path">File Name</param>
        /// <returns>True if successful saving the maze</returns>
        public bool saveMaze(string mazeName, string filePath)
        {
            WinMaze winMaze = m_currentWinMaze;
            Maze3d currentMaze = winMaze.getMaze();
            add3dMaze(mazeName, currentMaze);
            if (isSolutionExists)
                m_solutionsDictionary[mazeName] = mCurrentSolution;

            if (currentMaze == null)
                return false;
            using (FileStream fileOutStream = new FileStream(filePath, FileMode.Create))
            {
                using (Stream outStream = new MyCompressorStream(fileOutStream))
                {
                    outStream.Write(currentMaze.toByteArray(), 0, currentMaze.toByteArray().Length);
                    outStream.Flush();
                }
            }
            printMaze(winMaze, winMaze.PosZ);
            return true;
        }

        /// <summary>
        /// Displays the solution in View
        /// </summary>
        public void showSolution()
        {
            string mazeTitle = currentMazeName;
            Solution solution = displaySolution(mazeTitle);
            if (null == solution)
            {
                ErrorOutput = ("ERROR: The solution does not exist for the maze named: " + mazeTitle + ".");
                MessageBox.Show(ErrorOutput, "Error");
            }
            else // soultion exists
            {
                List<string[]> tempList = solution.getSolutionCoordinates();
                MessageBox.Show(solution.GetSolutionPathByString());
                staticMaze.setSolutionCoordinates(tempList);
                m_currentWinMaze.setSolutionCoordinates(tempList);
            }
            WinMaze winMaze = getWinMaze("maze");
            printMaze(winMaze, winMaze.PosZ);
        }

        /// <summary>
        /// Hides the solution in View
        /// </summary>
        public void hideSolution()
        {
            staticMaze = originalWinMaze;
            solExists = true;
            printMaze(staticMaze, staticMaze.PosZ);
        }

        /// <summary>
        /// Opens the load maze dialog to the user in View
        /// </summary>
        public void loadClicked()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string filePath = "";
            string fileTitle = "";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                fileTitle = openFileDialog.SafeFileName;
                loadMaze(filePath.ToLower(), fileTitle);
                isMazeExists = true;
                solExists = false;
            }
        }

        /// <summary>
        /// Opens the save maze dialog to the user in View
        /// </summary>
        public void saveClicked()
        {
            if (!isMazeExists)
            {
                ErrorOutput = ("Please generate a maze");
                MessageBox.Show(ErrorOutput, "Error");
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string filePath = "";
            string fileTitle = "";
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
                fileTitle = saveFileDialog.SafeFileName;
                filePath += ".maze";
                currentMazeName = fileTitle;
                if (saveMaze(fileTitle, filePath))
                {
                    Output = ("Maze saved successfuly\nPath: | " + filePath + " |");
                    MessageBox.Show(Output, "Message");
                }
            }
        }

        /// <summary>
        /// Load Maze - Loads a maze to the memory with a given file path
        /// from the user.
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <param name="mazeName">Name of the maze</param>
        public void loadMaze(string path, string mazeName)
        {
            mazeName = mazeName.Substring(0, mazeName.Length - 5);
            currentMazeName = mazeName;
            int numberOfBytes;
            byte[] compressionArray = new byte[100];
            List<byte> compressedList = new List<byte>();
            using (FileStream fileInStream = new FileStream(path, FileMode.Open))
            {
                using (Stream compressor = new MyCompressorStream(fileInStream))
                {
                    while ((numberOfBytes = compressor.Read(compressionArray, 0, 100)) != 0)
                    {
                        for (int i = 0; i < numberOfBytes; i++)
                        {
                            compressedList.Add(compressionArray[i]);
                        }
                    }
                }
            }
            Maze3d loadedMaze = new Maze3d(compressedList.ToArray());
            add3dMaze(mazeName, loadedMaze);
            WinMaze winMaze = new WinMaze(loadedMaze, "maze", 50);
            m_currentWinMaze = winMaze;
            m_winMazesDictionary.Remove("maze");
            m_winMazesDictionary["maze"] = winMaze;
            printMaze(winMaze, winMaze.PosZ);
        }

        /// <summary>
        /// Solve Maze - Solves the given maze with a given
        /// solving algorithm - BFS/DFS
        /// </summary>
        /// <param name="mazeName">Maze name</param>
        /// <param name="solvingAlgorithm">Solving algorithm - BFS/DFS</param>
        public void solveMaze(string solvingAlgorithm)
        {
            string mazeName = currentMazeName;
            ISearchingAlgorithm searchingAlgorithm = null;
            if (solvingAlgorithm == "DFS")
            {
                searchingAlgorithm = new DFS();
            }
            else if (solvingAlgorithm == "BFS")
            {
                searchingAlgorithm = new BFS();
            }
            else
            {
                return;
            }

            var resetEvent = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
            {
                solve(mazeName, solvingAlgorithm);
                resetEvent.Set();
            }));
            resetEvent.WaitOne();
        }

        /// <summary>
        /// Thread solving of the maze - to be implemented in a thread
        /// </summary>
        /// <param name="searchingAlgorithm">Chosen search algorithm</param>
        /// <param name="mazeName">Maze name</param>
        private void solve(string mazeName, string solvingAlgorithm)
        {
            CommunicateWithServer(mazeName, solvingAlgorithm);
        }

        /// <summary>
        /// Display Solution - Displays the solution of the command
        /// </summary>
        /// <param name="mazeName">Maze name</param>
        /// <returns>Solution type object containing the solution if exists</returns>
        public Solution displaySolution(string mazeName)
        {
            Solution mazeSolution;
            try
            {
                mazeSolution = m_solutionsDictionary[mazeName];
            }
            catch (Exception)
            {
                return null;
            }
            return mazeSolution;
        }

        /// <summary>
        /// Returns true if the maze is found on the memory (dictionary)
        /// </summary>
        /// <param name="mazeName">Maze name</param>
        /// <returns>True/False if the maze is found</returns>
        public bool mazeExists(string mazeName)
        {
            return m_mazesDictionary.ContainsKey(mazeName);
        }

        /// <summary>
        /// Exit - Quits the program safely & closes
        /// all open thread and processes
        /// </summary>
        public void exit()
        {
            Stop();
        }

        /// <summary>
        /// Returns the maze by a given name
        /// </summary>
        /// <param name="mazeName">Maze name</param>
        /// <returns>If found - the maze. If not - Null</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private Maze3d getMazeByName(string mazeName)
        {
            if (!mazeExists(mazeName))
            {
                return null;
            }
            else
            {
                return m_mazesDictionary[mazeName];
            }
        }

        /// <summary>
        /// Adds the given maze to the dictionary (memory)
        /// </summary>
        /// <param name="mazeName">Maze Name</param>
        /// <param name="currentMaze">Maze</param>
        private void add3dMaze(string mazeName, Maze3d currentMaze)
        {
            m_mazesDictionary["maze"] = currentMaze;
            m_mazesDictionary[mazeName] = currentMaze;
        }

        /// <summary>
        /// Stops safely all running threads
        /// </summary>
        public void Stop()
        {
            foreach (var item in m_stoppingList)
            {
                item.Stop();
            }
        }

        /// <summary>
        /// Returns the WinMaze by name
        /// </summary>
        /// <param name="name">Name of the maze</param>
        /// <returns>WinMaze object</returns>
        public WinMaze getWinMaze(string name)
        {
            if (m_winMazesDictionary.ContainsKey(name))
                return m_winMazesDictionary[name];
            else
                return null;
        }

        /// <summary>
        /// Move left in the maze command
        /// </summary>
        public void moveLeft()
        {
            WinMaze maze = m_winMazesDictionary["maze"];
            int[] currentCellWalls = maze.getMaze().getMazeByFloor(maze.PosZ).getCell(maze.PosX, maze.PosY).getWallsAroundCell();
            if (maze.PosX - 1 >= 0)
            {
                int[] currentCellWallsLeft = maze.getMaze().getMazeByFloor(maze.PosZ).getCell(maze.PosX - 1, maze.PosY).getWallsAroundCell();
                if ((currentCellWalls[0] == 0) || (currentCellWallsLeft[1] == 0))
                {
                    maze.PosX -= 1;
                    m_winMazesDictionary[maze.getName()] = maze;
                    m_currentWinMaze = maze;
                    printMaze(maze, maze.PosZ);
                }
            }
        }

        /// <summary>
        /// Move right in the maze command
        /// </summary>
        public void moveRight()
        {
            WinMaze maze = m_winMazesDictionary["maze"];
            int[] currentCellWalls = maze.getMaze().getMazeByFloor(maze.PosZ).getCell(maze.PosX, maze.PosY).getWallsAroundCell();
            if (maze.PosX + 1 < maze.getMaze().MyColumns)
            {
                if (currentCellWalls[1] == 0)
                {
                    maze.PosX += 1;
                    m_winMazesDictionary[maze.getName()] = maze;
                    m_currentWinMaze = maze;
                    printMaze(maze, maze.PosZ);
                }
            }
        }

        /// <summary>
        /// Move back in the maze command
        /// </summary>
        public void moveBack()
        {
            WinMaze maze = m_winMazesDictionary["maze"];
            int[] currentCellWalls = maze.getMaze().getMazeByFloor(maze.PosZ).getCell(maze.PosX, maze.PosY).getWallsAroundCell();
            if (maze.PosY - 1 >= 0)
            {
                int[] currentCellWallsBack = maze.getMaze().getMazeByFloor(maze.PosZ).getCell(maze.PosX, maze.PosY - 1).getWallsAroundCell();
                if ((currentCellWalls[2] == 0) || (currentCellWallsBack[3] == 0))
                {
                    maze.PosY -= 1;
                    m_winMazesDictionary[maze.getName()] = maze;
                    m_currentWinMaze = maze;
                    printMaze(maze, maze.PosZ);
                }
            }
        }

        /// <summary>
        /// Move forward in the maze command
        /// </summary>
        public void moveForward()
        {
            WinMaze maze = m_winMazesDictionary["maze"];
            int[] currentCellWalls = maze.getMaze().getMazeByFloor(maze.PosZ).getCell(maze.PosX, maze.PosY).getWallsAroundCell();
            if (maze.PosY + 1 < maze.getMaze().MyRows)
            {
                if (currentCellWalls[3] == 0)
                {
                    maze.PosY += 1;
                    m_winMazesDictionary[maze.getName()] = maze;
                    m_currentWinMaze = maze;
                    printMaze(maze, maze.PosZ);
                }
            }
        }

        /// <summary>
        /// Move down in the maze command
        /// </summary>
        public void moveDown()
        {
            WinMaze maze = m_winMazesDictionary["maze"];
            if (maze.PosZ - 1 >= 0)
            {
                int isBrockBelow = maze.getMaze().getMazeByFloor(maze.PosZ - 1).getCell(maze.PosX, maze.PosY).BlockOrEmpty;
                if (isBrockBelow == 0)
                {
                    maze.PosZ -= 1;
                    m_winMazesDictionary[maze.getName()] = maze;
                    m_currentWinMaze = maze;
                    printMaze(maze, maze.PosZ);
                }
            }
        }

        /// <summary>
        /// Move up in the maze command
        /// </summary>
        public void moveUp()
        {
            WinMaze maze = m_winMazesDictionary["maze"];
            if ((maze.PosZ + 1) < maze.getMaze().MyHeight)
            {
                int isBrockAbove = maze.getMaze().getMazeByFloor(maze.PosZ + 1).getCell(maze.PosX, maze.PosY).BlockOrEmpty;
                if (isBrockAbove == 0)
                {
                    maze.PosZ += 1;
                    m_winMazesDictionary[maze.getName()] = maze;
                    m_currentWinMaze = maze;
                    printMaze(maze, maze.PosZ);
                }
            }
        }

        /// <summary>
        /// Responsible of communicating with the server
        /// </summary>
        /// <param name="mazeName">Name of the maze</param>
        /// <param name="solvingAlgorithm">Solving Algorithm</param>
        private void CommunicateWithServer(string mazeName, string solvingAlgorithm)
        {
            Output = ("Searching for the maze named: " + mazeName + "\nWith the searching algorithm: " + solvingAlgorithm);
            MessageBox.Show(Output, "Message");

            int port = Properties.Settings.Default.Port;
            String serverIP = Properties.Settings.Default.IP;
            try
            {
                IFormatter binaryFormatter = new BinaryFormatter();

                Object[] arrayToSend = new object[3];
                arrayToSend[0] = m_mazesDictionary[mazeName];
                arrayToSend[1] = mazeName;
                arrayToSend[2] = solvingAlgorithm;

                Object[] receivedArray = new object[2];

                using (TcpClient client = new TcpClient(serverIP, port))
                {
                    using (NetworkStream clientStream = client.GetStream())
                    {
                        binaryFormatter.Serialize(clientStream, arrayToSend);
                        receivedArray = (Object[])binaryFormatter.Deserialize(clientStream);
                    }
                }
                Solution receivedSolution = (Solution)receivedArray[0];
                bool wasTheMazeSolved = (bool)receivedArray[1];
                m_solutionsDictionary[mazeName] = receivedSolution;
                mCurrentSolution = receivedSolution;
                isSolutionExists = true;

                Output = ("Received a solution from server." + "\nWas it taken from the cache: " + wasTheMazeSolved);
                MessageBox.Show(Output, "Message");
            }
            catch (Exception e)
            {
                ErrorOutput = ("Error communicating with server at: " + serverIP + ". Exception: " + e.Message);
                MessageBox.Show(ErrorOutput, "Error");
            }
        }

        /// <summary>
        /// Creates a new maze display
        /// </summary>
        public void newCanvas()
        {
            isSolutionExists = false;
            isMazeExists = false;
            CurrentMazeCanvas = new Canvas();
            SecondaryMazeCanvas = new Canvas();
        }

        #region Properties

        private string mLevelData = "3";

        public string LevelData
        {
            get { return mLevelData; }
            set { mLevelData = value; NotifyPropertyChanged("LevelData"); }
        }

        private string mColumnData = "6";

        public string ColumnData
        {
            get { return mColumnData; }
            set { mColumnData = value; NotifyPropertyChanged("ColumnData"); }
        }

        private string mRowData = "6";

        public string RowData
        {
            get { return mRowData; }
            set { mRowData = value; NotifyPropertyChanged("RowData"); }
        }

        private string mCellSizeData = "50";

        public string CellSizeData
        {
            get { return mCellSizeData; }
            set { mCellSizeData = value; NotifyPropertyChanged("CellSizeData"); }
        }

        private Canvas mCurrentCanvas;

        public Canvas CurrentMazeCanvas
        {
            get { return mCurrentCanvas; }
            set { mCurrentCanvas = value; NotifyPropertyChanged("CurrentMazeCanvas"); }
        }

        private Canvas mSecondaryMazeCanvas;

        public Canvas SecondaryMazeCanvas
        {
            get { return mSecondaryMazeCanvas; }
            set { mSecondaryMazeCanvas = value; NotifyPropertyChanged("SecondaryMazeCanvas"); }
        }

        private string mOutput;

        public string Output
        {
            get { return mOutput; }
            set { mOutput = value; NotifyPropertyChanged("Output"); }
        }

        private string mErrorOutput;

        public string ErrorOutput
        {
            get { return mErrorOutput; }
            set { mErrorOutput = value; NotifyPropertyChanged("ErrorOutput"); }
        }

        #endregion Properties
    }
}