using Microsoft.EntityFrameworkCore;
using Orders.IServiceAPI;
using Orders.Model;
using Orders.ServiceAPI;
using Orders.Services;

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
        services.AddScoped<IWebClient, WebClient>();
        services.AddSingleton<IEventService, EventService>();
        services.AddSingleton<IEventHandlerService, EventHandlerService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddHostedService<QueuedHostedService>();
        services.AddSingleton<IBackgroundTaskQueue>(ctx =>
        {
            return new BackgroundTaskQueue(100);
        });
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(@"Data Source=DESKTOP-1QPLQOE\TEW_SQLEXPRESS;Initial Catalog=aspnet-Orders;Integrated Security=True;Pooling=False;TrustServerCertificate=True"), ServiceLifetime.Transient);
    }
}