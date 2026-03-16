using Microsoft.EntityFrameworkCore;
using ServerManager.Models;
namespace ServerManager.Data;

public class ServerDbContext : DbContext
{
    public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options) { }
    public DbSet<Server> Servers => Set<Server>();
}