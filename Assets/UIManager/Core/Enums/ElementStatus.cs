namespace UIManager
{
    /// <summary>
    /// Represents the status of a UI element, indicating its current state in a lifecycle.
    /// </summary>
    public enum ElementStatus
    {
        /// <summary>
        /// Indicates that the UI element is completely closed and not visible to the user.
        /// </summary>
        Closed,

        /// <summary>
        /// Indicates that the UI element is in the process of closing, but not yet fully closed.
        /// This state can be used to manage animations or logic that occur as the element closes.
        /// </summary>
        Closing,

        /// <summary>
        /// Indicates that the UI element is fully opened and visible to the user.
        /// </summary>
        Opened,

        /// <summary>
        /// Indicates that the UI element is in the process of opening, but not yet fully open.
        /// This state can be used to manage animations or logic that occur as the element opens.
        /// </summary>
        Opening
    }
}
