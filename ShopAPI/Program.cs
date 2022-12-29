using Microsoft.EntityFrameworkCore;
using ShopAPI.Model;
using ShopAPI.ServiceAPI;
using ShopAPI.Services;

internal class Program
{
    public const string allowpolicy = "_localallowpolicy";
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

        app.UseCors(allowpolicy);

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapGet("/", () => $"App: {app.Environment.ApplicationName}, version: {app.Services.GetService<IVersionService>()?.Version ?? "Unknown"}");

        app.Run();
    }

    private static void configureServices(IServiceCollection services)
    {
        services.AddTransient<IVersionService, VersionService>();
        services.AddTransient<IWebClient, WebClient>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCors(options =>
        {
            options.AddPolicy(name: allowpolicy,
                policy =>
                {
                    policy.WithOrigins("http://localhost:4200");
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
        });
    }
}