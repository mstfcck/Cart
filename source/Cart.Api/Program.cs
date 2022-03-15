using Cart.Api.Extensions;
using Cart.Api.Middlewares;
using Cart.Application;
using Cart.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();

builder.Services.AddOptions();

// Application
builder.Services.AddApplication();

// Infrastructure
builder.Services.AddInfrastructure();

builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();

// Logger
builder.AddLogger();

try
{
    // Redis
    builder.Services.AddRedis(builder.Configuration);

    builder.Services.AddSwagger();

    var app = builder.Build();

    app.UseMiddleware<ExceptionMiddleware>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
}