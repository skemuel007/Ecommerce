using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(); 
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public static async Task ConfigureWebApplicationServices(this WebApplication app, IConfiguration configuration)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<EcommerceContext>();
// Create an instance of ILogger specifically for EcommerceContextSeed
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<EcommerceContextSeed>();

        try
        {
            await context.Database.MigrateAsync();
            // Create an instance of EcommerceContextSeed
            var ecommerceContextSeed = new EcommerceContextSeed(logger);
            await ecommerceContextSeed.SeedDataAsync(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occured during migration");
        }

        app.Run();
    }
}