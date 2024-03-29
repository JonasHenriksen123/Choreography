using Microsoft.EntityFrameworkCore;
using Stock.Model;
using Stock.ServiceAPI;
using Stock.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        configureServices(builder.Services);
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapGet("/", () => $"App: {app.Environment.ApplicationName}, version: {app.Services.GetService<IVersionService>()?.Version ?? "Unknown"}");

        app.Run();
    }
    private static void configureServices(IServiceCollection services)
    {
        services.AddScoped<IVersionService, VersionService>();
        services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(@"Data Source=DESKTOP-1QPLQOE\TEW_SQLEXPRESS;Initial Catalog=aspnet-Stock;Integrated Security=True;Pooling=False;TrustServerCertificate=True"), ServiceLifetime.Transient);
        services.AddScoped<IWebClient, WebClient>();
        services.AddHostedService<QueuedHostedService>();
        services.AddSingleton<IBackgroundTaskQueue>(ctx =>
        {
            return new BackgroundTaskQueue(100);
        });
        services.AddSingleton<IEventService, EventService>();
        services.AddSingleton<IEventHandlerService, EventHandlerService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}