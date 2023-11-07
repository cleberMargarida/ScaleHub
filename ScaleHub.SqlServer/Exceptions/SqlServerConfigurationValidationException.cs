using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleHub.SqlServer.Exceptions
{
    internal class SqlServerConfigurationValidationException : Exception
    {
        public SqlServerConfigurationValidationException(string? message) : base(message)
        {
        }
    }
}
