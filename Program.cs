using Microsoft.EntityFrameworkCore;
using ServerManager.Data;
using ServerManager.Services;
using ServerManager.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ServerService>();

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<ServerDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "v1";
    config.Version = "v1";
    config.Title = "ServerAPI";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
    app.ApplyMigrations();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();

