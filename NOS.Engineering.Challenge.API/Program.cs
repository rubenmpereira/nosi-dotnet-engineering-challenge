using NOS.Engineering.Challenge.API.Extensions;

var builder = WebApplication.CreateBuilder(args)
        .ConfigureWebHost()
        .RegisterServices();
var app = builder.Build();

app.MapControllers();
app.UseSwagger()
    .UseSwaggerUI();

app.UseResponseCaching();

using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
    .SetMinimumLevel(LogLevel.Trace)
    .AddConsole());

ILogger logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("App is running");

app.Run();