using ScaleHub.Core;

internal class HostedConsole : BackgroundService
{
    private readonly IScaleHub hub;

    public HostedConsole(IScaleHub hub)
    {
        this.hub = hub;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var context = await hub.GetContextAsync();
        await WriteContext(context);
    }

    public static Task WriteContext(ScaleContext context)
    {
        Console.WriteLine("Actual: {0}", context.Actual);
        Console.WriteLine("Replicas: {0}", context.Replicas);
        return Task.CompletedTask;
    }
}