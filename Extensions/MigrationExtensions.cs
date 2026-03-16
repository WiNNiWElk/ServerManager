using Microsoft.EntityFrameworkCore;
using Npgsql.Replication;
using ServerManager.Data;

namespace ServerManager.Extensions;
public static class MigrationsExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ServerDbContext>();

        dbContext.Database.Migrate();
    }
}