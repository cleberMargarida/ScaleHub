using System.Text;

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

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastUpdate { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append('{');
            builder.Append(' ');

            builder.Append(nameof(Id));
            builder.Append(" = ");
            builder.Append(Id);
            builder.Append(", ");
            
            builder.Append(nameof(HostName));
            builder.Append(" = ");
            builder.Append(HostName);
            builder.Append(", ");
            
            builder.Append(nameof(Ip));
            builder.Append(" = ");
            builder.Append(Ip);
            builder.Append(", ");
            
            builder.Append(nameof(Tag));
            builder.Append(" = ");
            builder.Append(Tag);
            builder.Append(", ");

            builder.Append(nameof(LastUpdate));
            builder.Append(" = ");
            builder.Append(LastUpdate);
            
            builder.Append(' ');
            builder.Append('}');
            return builder.ToString();
        }
    }
}
