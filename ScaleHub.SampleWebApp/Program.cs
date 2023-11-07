using ScaleHub.SqlServer.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScaleHub(opt =>
{
    opt.UseSqlServer(s => 
    {
        s.ConnString = builder.Configuration.GetConnectionString("ScaleHub");
        s.PeriodicallyCheck(p =>
        {
            p.Period = TimeSpan.FromMinutes(1);
            p.MaxDiffAllowed = TimeSpan.FromMinutes(2);
        });
    });
    opt.ConfigureSubs(e =>
    {
        e.OnSubscribing += HostedConsole.WriteContext;
        e.OnUnsubscribing += HostedConsole.WriteContext;
    });
});

builder.Services.AddHostedService<HostedConsole>();

var app = builder.Build();

app.Run();
