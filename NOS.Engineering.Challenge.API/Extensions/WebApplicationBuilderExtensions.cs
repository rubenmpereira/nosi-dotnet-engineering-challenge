using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using NOS.Engineering.Challenge.Database;
using NOS.Engineering.Challenge.Managers;
using NOS.Engineering.Challenge.Models;
using Microsoft.EntityFrameworkCore;
using NOS.Engineering.Challenge.API.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace NOS.Engineering.Challenge.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder webApplicationBuilder)
    {
        var serviceCollection = webApplicationBuilder.Services;

        serviceCollection.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.PropertyNamingPolicy = null;
        });
        serviceCollection.AddControllers();
        serviceCollection
            .AddEndpointsApiExplorer();

        serviceCollection.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nos Challenge Api", Version = "v1" });
        });

        var connectionString = webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        serviceCollection.AddResponseCaching();

        serviceCollection
            //.RegisterSlowDatabase()
            .RegisterContentsManager()
            .RegisterEntityFrameworkDatabase(connectionString);
        return webApplicationBuilder;
    }

    private static IServiceCollection RegisterSlowDatabase(this IServiceCollection services)
    {
        services.AddSingleton<IDatabase<Content, ContentDto>,SlowDatabase<Content, ContentDto>>();
        services.AddSingleton<IMapper<Content, ContentDto>, ContentMapper>();
        services.AddSingleton<IMockData<Content>, MockData>();

        return services;
    }

    private static IServiceCollection RegisterEntityFrameworkDatabase(this IServiceCollection services,string connectionString)
    {
        services.AddSingleton<IDatabase<Content, ContentDto>, EntityFrameworkDatabase<Content, ContentDto>>();
        services.AddSingleton<IMapper<Content, ContentDto>, ContentMapper>();
        services.AddDbContextFactory<ContentDbContext>(options => options.UseSqlServer(connectionString));

        return services;
    }

    private static IServiceCollection RegisterContentsManager(this IServiceCollection services)
    {
        services.AddSingleton<IContentsManager, ContentsManager>();

        return services;
    }
    
    
    public static WebApplicationBuilder ConfigureWebHost(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder
            .WebHost
            .ConfigureLogging(logging => { logging.ClearProviders(); });

        return webApplicationBuilder;
    }
}