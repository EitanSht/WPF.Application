namespace Client.View
{
    public delegate void func();

    /// <summary>
    /// Interface that will be implemented in order to use the View
    /// as part of the MVP module
    /// </summary>
    internal interface IView
    {
        /// <summary>
        /// Stream output method - Prints to the screen using a message box
        /// </summary>
        /// <param name="output">String with data to be presented</param>
        void Output();

        /// <summary>
        /// Error Stream output method - Prints to the screen using a message box
        /// </summary>
        /// <param name="errorOutput">String with data to be presented</param>
        void errorOutput();

        /// <summary>
        /// Exists safely from the current View
        /// </summary>
        void exit();
    }
}