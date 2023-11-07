using System.Diagnostics.CodeAnalysis;

namespace ScaleHub.SqlServer.Configurations
{
    internal readonly struct LastUpdateComparer : IEqualityComparer<DateTime>
    {
        private readonly long maxDiffAllowedTicks;

        public LastUpdateComparer(TimeSpan maxDiffAllowed)
        {
            maxDiffAllowedTicks = maxDiffAllowed.Ticks;
        }

        public bool Equals(DateTime x, DateTime y)
        {
            return maxDiffAllowedTicks >= Math.Abs((x - y).Ticks);
        }

        public int GetHashCode([DisallowNull] DateTime obj)
        {
            return 0;
        }
    }
}