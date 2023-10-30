namespace ScaleHub.Core
{
    /// <summary>
    /// Represents a context for scaling operations.
    /// </summary>
    public class ScaleContext
    {
        /// <summary>
        /// Gets or sets the number of replicas being used across the system.
        /// </summary>
        public int Replicas { get; set; }

        /// <summary>
        /// Gets or sets the actual instance number index based 1.
        /// </summary>
        public int Actual { get; set; }
    }
}
