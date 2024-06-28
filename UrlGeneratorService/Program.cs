using UrlGeneratorService;
using UrlGeneratorService.Data;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    })
    .Build();

// Initialize database
using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<UrlDbContext>();
    context.Database.EnsureCreated();
}

host.Run();
