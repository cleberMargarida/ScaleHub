namespace ScaleHub.Core
{
    /// <summary>
    /// Represents information about a server.
    /// </summary>
    internal class ServerInfo
    {
        /// <summary>
        /// Gets or sets the unique identifier of the server.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the hostname of the server.
        /// </summary>
        public string HostName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the IP address of the server.
        /// </summary>
        public string Ip { get; set; } = default!;

        /// <summary>
        /// Gets or sets the Tag to differ the service.
        /// </summary>
        public string Tag { get; set; } = default!;
    }
}
