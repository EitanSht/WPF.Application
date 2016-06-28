using Compression;
using MazeGenerators;
using Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Wpf.ATP.Project.Model
{
    /// <summary>
    /// Model part of the MVP module
    /// </summary>
    internal class MyModel : IModel
    {
        public event func ModelChanged;

        private Dictionary<string, Maze3d> m_mazesDictionary;
        private Dictionary<string, Solution> m_solutionsDictionary;
        private Dictionary<string, WinMaze> m_winMazesDictionary;
        private WinMaze m_currentWinMaze;
        private string directoryPath = Directory.GetCurrentDirectory() + ("\\");
        private List<IStoppable> m_stoppingList = new List<IStoppable>();
        private List<string> m_instructions;
        private bool m_initiatedStop = false;
        private Solution mCurrentSolution = new Solution();
        private bool isSolutionExists = false;

        /// <summary>
        /// MyModel constructor
        /// </summary>
        /// <param name="controller">Controller</param>
        public MyModel()
        {
            ActivateThreadPool();

            unZipDictionaries(directoryPath);

            if (isMazesFileExists())
                loadMazeDictionary();
            else
                m_mazesDictionary = new Dictionary<string, Maze3d>();

            if (isSolutionFileExists())
                loadSolutionDictionary();
            else
                m_solutionsDictionary = new Dictionary<string, Solution>();
            m_winMazesDictionary = new Dictionary<string, WinMaze>();
            m_instructions = new List<string>();
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
        /// Directory Command - Returns file names & paths
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <returns>If path exists - files & directories, if not - error message</returns>
        public string[] getDir(string filePath)
        {
            return Directory.GetFileSystemEntries(filePath);
        }

        /// <summary>
        /// Generate a 3d Maze - Generates a new maze with given user input
        /// </summary>
        /// <param name="name">Maze Name</param>
        /// <param name="levels">How many levels to the maze</param>
        /// <param name="columns">How many columns to the maze</param>
        /// <param name="rows">How many rows to the maze</param>
        [STAThread]
        public void generate3dMaze(string mazeName, int levels, int columns, int rows, int size)
        {
            int[] mazeDimentions = { levels, columns, rows };
            int cellSize = size;

            var resetEvent = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
            {
                threadGenerateMaze(mazeDimentions, mazeName, cellSize);
                resetEvent.Set();
            }));
            resetEvent.WaitOne();
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
            isSolutionExists = false;
            saveMazeDictionary();
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
            return true;
        }

        /// <summary>
        /// Load Maze - Loads a maze to the memory with a given file path
        /// from the user.
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <param name="mazeName">Name of the maze</param>
        public void loadMaze(string path, string mazeName)
        {
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
            //if (!winMaze.isSolutionExists())
            //    isSolutionExists = false;
            m_currentWinMaze = winMaze;
            m_winMazesDictionary.Remove("maze");
            m_winMazesDictionary["maze"] = winMaze;
            m_instructions.Clear();
            m_instructions.Add("display");
            m_instructions.Add("maze");
        }

        /// <summary>
        /// Maze Size - Returns the size of the maze object in the memory
        /// </summary>
        /// <param name="mazeName">Maze name</param>
        /// <returns>The size in bytes of the maze object in the memory</returns>
        public int mazeSize(string mazeName)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            byte[] returnedStream;

            Maze3d currentMaze = getMazeByName(mazeName);
            if (currentMaze == null)
            {
                return -1;
            }
            binaryFormatter.Serialize(memoryStream, currentMaze);
            returnedStream = memoryStream.ToArray();

            return returnedStream.Length;
        }

        /// <summary>
        /// File Size - Returns the size of the file in bytes
        /// </summary>
        /// <param name="Path">Directory path</param>
        /// <returns>Size in bytes of the given file name in the disc</returns>
        public long fileSize(string filePath)
        {
            FileInfo fileInformation = new FileInfo(filePath);
            return fileInformation.Length;
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
            //MessageBox.Show("exists? " + m_mazesDictionary.ContainsKey(mazeName).ToString());
            return m_mazesDictionary.ContainsKey(mazeName);
        }

        /// <summary>
        /// Returns true if the solution exists
        /// </summary>
        /// <param name="mazeName"></param>
        /// <returns>True if the solution exists</returns>
        public bool solutionExists(string mazeName)
        {
            return m_solutionsDictionary.ContainsKey(mazeName);
        }

        /// <summary>
        /// Exit - Quits the program safely & closes
        /// all open thread and processes
        /// </summary>
        public void exit()
        {
            if (!m_initiatedStop)
                zipDictionaries(directoryPath);
            ModelChanged();
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
            try
            {
                //m_solutionsDictionary.Remove("maze");
            }
            catch (Exception)
            {
                // Empty implementation
            }
            //MessageBox.Show("add");
            m_mazesDictionary["maze"] = currentMaze;
            m_mazesDictionary[mazeName] = currentMaze;
        }

        /// <summary>
        /// Activates the event 'Model Changed'
        /// </summary>
        public void modelEvent()
        {
            ModelChanged();
        }

        /// <summary>
        /// Saves the current solution to the disc
        /// </summary>
        public void saveSolutionDictionary()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                FileStream writerFileStream =
                    new FileStream(directoryPath + "\\solutions.dat", FileMode.Create, FileAccess.Write);
                formatter.Serialize(writerFileStream, m_solutionsDictionary);
                writerFileStream.Close();
            }
            catch (Exception)
            {
                // No implementation
            }
        }

        /// <summary>
        /// Loads the solutions from the disc
        /// </summary>
        public void loadSolutionDictionary()
        {
            Dictionary<string, Solution> loadedSolutionFile = new Dictionary<string, Solution>();
            BinaryFormatter formatter = new BinaryFormatter();

            if (File.Exists(directoryPath + "\\solutions.dat"))
            {
                try
                {
                    FileStream readerFileStream = new FileStream(directoryPath + "\\solutions.dat", FileMode.Open, FileAccess.Read);
                    loadedSolutionFile = (Dictionary<string, Solution>)formatter.Deserialize(readerFileStream);
                    readerFileStream.Close();

                    m_solutionsDictionary = loadedSolutionFile;
                }
                catch (Exception)
                {
                    // No implementation
                }
            }
        }

        /// <summary>
        /// Saves the mazes dictionary to the disc
        /// </summary>
        public void saveMazeDictionary()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            //MessageBox.Show("save");
            try
            {
                FileStream writerFileStream =
                    new FileStream(directoryPath + "\\mazes.dat", FileMode.Create, FileAccess.Write);
                formatter.Serialize(writerFileStream, m_mazesDictionary);
                writerFileStream.Close();
            }
            catch (Exception)
            {
                // No implementation
            }
        }

        /// <summary>
        /// Loads the mazes dictionary from the disc
        /// </summary>
        public void loadMazeDictionary()
        {
            Dictionary<string, Maze3d> loadedMazesFile = new Dictionary<string, Maze3d>();
            BinaryFormatter formatter = new BinaryFormatter();
            if (File.Exists(directoryPath + "\\mazes.dat"))
            {
                try
                {
                    FileStream readerFileStream = new FileStream(directoryPath + "\\mazes.dat", FileMode.Open, FileAccess.Read);
                    loadedMazesFile = (Dictionary<string, Maze3d>)formatter.Deserialize(readerFileStream);
                    readerFileStream.Close();
                    m_mazesDictionary = loadedMazesFile;
                }
                catch (Exception)
                {
                    // No implementation
                }
            }
        }

        /// <summary>
        /// Unzips the files from the disc and creates & retrieves the dictionaries
        /// </summary>
        /// <param name="directoryPath">Directory path</param>
        public void unZipDictionaries(string directoryPath)
        {
            DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);
            if ((File.Exists(directoryPath + "\\mazes.dat.zip")) || (File.Exists(directoryPath + "\\solutions.dat.zip")))
                foreach (FileInfo fileToDecompress in directorySelected.GetFiles("*.dat.zip"))
                {
                    using (FileStream originalFileStream = fileToDecompress.OpenRead())
                    {
                        string currentFileName = fileToDecompress.FullName;
                        string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                        using (FileStream decompressedFileStream = File.Create(newFileName))
                        {
                            using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                            {
                                decompressionStream.CopyTo(decompressedFileStream);
                            }
                        }
                    }
                }
        }

        /// <summary>
        /// Compresses (Zip) the dictionaries and saves them to the disc
        /// </summary>
        /// <param name="directoryPath">Directory path</param>
        public void zipDictionaries(string directoryPath)
        {
            DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);

            foreach (FileInfo fileToCompress in directorySelected.GetFiles())
            {
                using (FileStream originalFileStream = fileToCompress.OpenRead())
                {
                    if ((File.GetAttributes(fileToCompress.FullName) &
                       FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension == ".dat")
                    {
                        using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".zip"))
                        {
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                               CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);
                            }
                        }
                        FileInfo info = new FileInfo(directoryPath + "\\" + fileToCompress.Name + ".zip");
                    }
                }
            }

            string[] filePaths = Directory.GetFiles(directoryPath);
            //foreach (string filePath in filePaths)
            //{
            //    var name = new FileInfo(filePath).Name;
            //    name = name.ToLower();
            //    if ((name == "mazes.dat") || (name == "solutions.dat"))
            //    {
            //        File.Delete(filePath);
            //    }
            //}
        }

        /// <summary>
        /// Returns true if the solution file exists
        /// </summary>
        /// <returns>True if the file exists</returns>
        public bool isSolutionFileExists()
        {
            return File.Exists(directoryPath + "\\solutions.dat");
        }

        /// <summary>
        /// Returns true if the mazes file exists
        /// </summary>
        /// <returns>True if the file exists</returns>
        public bool isMazesFileExists()
        {
            return File.Exists(directoryPath + "\\mazes.dat");
        }

        /// <summary>
        /// Stops safely all running threads
        /// </summary>
        public void Stop()
        {
            m_initiatedStop = true;
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
        /// <param name="maze">WinMaze current maze</param>
        public void moveLeft(WinMaze maze)
        {
            m_instructions.Clear();
            int[] currentCellWalls = maze.getMaze().getMazeByFloor(maze.PosZ).getCell(maze.PosX, maze.PosY).getWallsAroundCell();
            if (maze.PosX - 1 >= 0)
            {
                int[] currentCellWallsLeft = maze.getMaze().getMazeByFloor(maze.PosZ).getCell(maze.PosX - 1, maze.PosY).getWallsAroundCell();
                if ((currentCellWalls[0] == 0) || (currentCellWallsLeft[1] == 0))
                {
                    maze.PosX -= 1;
                    m_winMazesDictionary[maze.getName()] = maze;
                    m_currentWinMaze = maze;
                    m_instructions.Add("display");
                    m_instructions.Add("maze");
                    ModelChanged();
                }
            }
        }

        /// <summary>
        /// Move right in the maze command
        /// </summary>
        /// <param name="maze">WinMaze current maze</param>
        public void moveRight(WinMaze maze)
        {
            m_instructions.Clear();
            int[] currentCellWalls = maze.getMaze().getMazeByFloor(maze.PosZ).getCell(maze.PosX, maze.PosY).getWallsAroundCell();
            if (maze.PosX + 1 < maze.getMaze().MyColumns)
            {
                if (currentCellWalls[1] == 0)
                {
                    maze.PosX += 1;
                    m_winMazesDictionary[maze.getName()] = maze;
                    m_currentWinMaze = maze;
                    m_instructions.Add("display");
                    m_instructions.Add("maze");
                    ModelChanged();
                }
            }
        }

        /// <summary>
        /// Move back in the maze command
        /// </summary>
        /// <param name="maze">WinMaze current maze</param>
        public void moveBack(WinMaze maze)
        {
            m_instructions.Clear();
            int[] currentCellWalls = maze.getMaze().getMazeByFloor(maze.PosZ).getCell(maze.PosX, maze.PosY).getWallsAroundCell();
            if (maze.PosY - 1 >= 0)
            {
                int[] currentCellWallsBack = maze.getMaze().getMazeByFloor(maze.PosZ).getCell(maze.PosX, maze.PosY - 1).getWallsAroundCell();
                if ((currentCellWalls[2] == 0) || (currentCellWallsBack[3] == 0))
                {
                    maze.PosY -= 1;
                    m_winMazesDictionary[maze.getName()] = maze;
                    m_currentWinMaze = maze;
                    m_instructions.Add("display");
                    m_instructions.Add("maze");
                    ModelChanged();
                }
            }
        }

        /// <summary>
        /// Move forward in the maze command
        /// </summary>
        /// <param name="maze">WinMaze current maze</param>
        public void moveForward(WinMaze maze)
        {
            m_instructions.Clear();
            int[] currentCellWalls = maze.getMaze().getMazeByFloor(maze.PosZ).getCell(maze.PosX, maze.PosY).getWallsAroundCell();
            if (maze.PosY + 1 < maze.getMaze().MyRows)
            {
                if (currentCellWalls[3] == 0)
                {
                    maze.PosY += 1;
                    m_winMazesDictionary[maze.getName()] = maze;
                    m_currentWinMaze = maze;
                    m_instructions.Add("display");
                    m_instructions.Add("maze");
                    ModelChanged();
                }
            }
        }

        /// <summary>
        /// Move down in the maze command
        /// </summary>
        /// <param name="maze">WinMaze current maze</param>
        public void moveDown(WinMaze maze)
        {
            m_instructions.Clear();
            if (maze.PosZ - 1 >= 0)
            {
                int isBrockBelow = maze.getMaze().getMazeByFloor(maze.PosZ - 1).getCell(maze.PosX, maze.PosY).BlockOrEmpty;
                if (isBrockBelow == 0)
                {
                    maze.PosZ -= 1;
                    m_winMazesDictionary[maze.getName()] = maze;
                    m_currentWinMaze = maze;
                    m_instructions.Add("display");
                    m_instructions.Add("maze");
                    ModelChanged();
                }
            }
        }

        /// <summary>
        /// Move up in the maze command
        /// </summary>
        /// <param name="maze">WinMaze current maze</param>
        public void moveUp(WinMaze maze)
        {
            m_instructions.Clear();
            if ((maze.PosZ + 1) < maze.getMaze().MyHeight)
            {
                int isBrockAbove = maze.getMaze().getMazeByFloor(maze.PosZ + 1).getCell(maze.PosX, maze.PosY).BlockOrEmpty;
                if (isBrockAbove == 0)
                {
                    maze.PosZ += 1;
                    m_winMazesDictionary[maze.getName()] = maze;
                    m_currentWinMaze = maze;
                    m_instructions.Add("display");
                    m_instructions.Add("maze");
                    ModelChanged();
                }
            }
        }

        /// <summary>
        /// Returns the current instructions
        /// </summary>
        /// <returns>String array of instructions</returns>
        public string[] getInstructions()
        {
            return m_instructions.ToArray();
        }

        /// <summary>
        /// Inserts the solution to the current maze
        /// </summary>
        /// <param name="name">Name of the maze</param>
        public void insertSolution(string name)
        {
            WinMaze winMaze = getWinMaze(name);
            Solution solution = m_solutionsDictionary[name];
        }
    }
}