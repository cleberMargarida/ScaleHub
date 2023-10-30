using ScaleHub.SqlServer.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScaleHub(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ScaleHub"));
    opt.ConfigureSubs(e =>
    {
        e.OnSubscribing += HostedConsole.WriteContext;
        e.OnUnsubscribing += HostedConsole.WriteContext;
    });
});

builder.Services.AddHostedService<HostedConsole>();

var app = builder.Build();

app.Run();
