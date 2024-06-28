using Microsoft.EntityFrameworkCore;
using UrlGeneratorService.Data;
using UrlGeneratorService.Repositories;
using UrlGeneratorService.Services;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<UrlDbContext>(options =>
            options.UseSqlite("Data Source=UrlGeneratorService.db"));

        services.AddScoped<IUrlMappingRepository, UrlMappingRepository>();
        services.AddScoped<IUrlShortenerService, UrlShortenerService>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Add CORS policy
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseDefaultFiles();

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UrlGeneratorService v1"));

        app.UseHttpsRedirection();
        app.UseRouting();

        // Use CORS policy
        app.UseCors();

        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<UrlDbContext>();
            context.Database.Migrate();
        }
    }
}
