namespace ScaleHub.SqlServer.UnitTests.Helpers;

using FluentAssertions;
using ScaleHub.Core;
using ScaleHub.SqlServer.Helpers;

public class LastUpdateComparerTests
{
    [Fact]
    public void GroupByLastUpdateComparer_With_2_WithinTheMaxDiffAllowed_ShouldContainOnly_2()
    {
        // Arrange
        var servers = new[]
        {
            new ServerInfo { LastUpdate = new DateTime(2023, 11, 01, 10, 10, 00) },
            new ServerInfo { LastUpdate = new DateTime(2023, 11, 01, 10, 05, 00) },
            new ServerInfo { LastUpdate = new DateTime(2023, 11, 01, 10, 00, 00) },
        };

        TimeSpan maxDiffAllowed = TimeSpan.FromMinutes(5);


        // Act
        var activesServers = servers.OrderByDescending(s => s.LastUpdate)
                                    .GroupBy(s => s.LastUpdate, new LastUpdateComparer(maxDiffAllowed))
                                    .First()
                                    .AsEnumerable();

        // Assert
        activesServers.Should().Contain(new[] { servers[0], servers[1] });
        activesServers.Should().NotContain(servers[2]);
    }

    [Fact]
    public void GroupByLastUpdateComparer_With_2_WithoutTheMaxDiffAllowed_ShouldContainOnly_1()
    {
        // Arrange
        var servers = new[]
        {
            new ServerInfo { LastUpdate = new DateTime(2023, 11, 01, 10, 10, 00) },
            new ServerInfo { LastUpdate = new DateTime(2023, 11, 01, 10, 03, 00) },
            new ServerInfo { LastUpdate = new DateTime(2023, 11, 01, 10, 00, 00) },
        };

        TimeSpan maxDiffAllowed = TimeSpan.FromMinutes(5);


        // Act
        var activesServers = servers.OrderByDescending(s => s.LastUpdate)
                                    .GroupBy(s => s.LastUpdate, new LastUpdateComparer(maxDiffAllowed))
                                    .First()
                                    .AsEnumerable();

        // Assert
        activesServers.Should().Contain(servers[0]);
        activesServers.Should().NotContain(new[] { servers[1], servers[2] });
    }
}
