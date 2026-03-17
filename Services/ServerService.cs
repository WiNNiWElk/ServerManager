using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ServerManager.Data;
using ServerManager.DTOs;
using ServerManager.Models;

namespace ServerManager.Services;



public class ServerService
{
    private readonly ILogger<ServerService> _logger;
    private readonly ServerDbContext _serverDbContext;

    public ServerService(ILogger<ServerService> logger, ServerDbContext serverDbContext)
    {
        _logger = logger;
        _serverDbContext = serverDbContext;
    }

    public async Task<Server> AddServer(CreateServerDto serverRequest)
    {
        var server = new Server
        {
            Id = Guid.NewGuid(),
            OsName = serverRequest.OsName,
            RamGb = serverRequest.RamGb,
            StorageGb = serverRequest.StorageGb,
            CpuCores = serverRequest.CpuCores,
            ServerStatus = ServerStatus.Off,
        };

        _serverDbContext.Servers.Add(server);

        _logger.LogInformation("Server added with ID: {id} ", server.Id);

        await _serverDbContext.SaveChangesAsync();

        return server;
    }

    public async Task<List<Server>> ShowAllServers()
    {
        var servers = await _serverDbContext.Servers.ToListAsync();

        return servers;
    }

    public async Task DeleteServer(Guid id)
    {
        var server = await _serverDbContext.Servers.FirstOrDefaultAsync(s => s.Id == id);

        if (server == null)
        {
            throw new Exception("Server not found");
        }

        _serverDbContext.Remove(server);

        _logger.LogInformation("Server has been deleted with id: {id}", server.Id);

        await _serverDbContext.SaveChangesAsync();
    }

    public async Task<List<Server>> FindAvailable(string osName, int? ramMemory, int? cpuCores)
    {
        var query = _serverDbContext.Servers.Where(s =>
        s.ServerStatus == ServerStatus.Ready || s.ServerStatus == ServerStatus.Off);

        if (!await query.AnyAsync())
        {
            throw new Exception("No servers available");
        }

        if (!string.IsNullOrEmpty(osName))
        {
            query = query.Where(s => s.OsName == osName);
        }

        if (ramMemory != null)
        {
            query = query.Where(s => s.RamGb >= ramMemory);
        }

        if (cpuCores != null)
        {
            query = query.Where(s => s.CpuCores >= cpuCores);
        }

        return await query.ToListAsync();
    }

    public async Task<Server> RentServer(Guid id)
    {
        var server = await _serverDbContext.Servers.FirstOrDefaultAsync(s => s.Id == id);

        if (server == null)
        {
            _logger.LogWarning("Server with id: {id} not found", id);
            throw new Exception("Server not found");
        }

        if (server.ServerStatus == ServerStatus.Ready)
        {
            server.ServerStatus = ServerStatus.Rented;
            server.RentedAt = DateTime.UtcNow;

            await _serverDbContext.SaveChangesAsync();
            return server;
        }

        if (server.ServerStatus == ServerStatus.Off)
        {
            server.ServerStatus = ServerStatus.Starting;
            server.StartedAt = DateTime.UtcNow;

            await _serverDbContext.SaveChangesAsync();
            return server;
        }

        throw new Exception("Server is not available");
    }

    public async Task<ServerResponseDto> ReleaseServer(Guid id)
    {
        var server = await _serverDbContext.Servers.FirstOrDefaultAsync(s => s.Id == id);

        if (server == null)
        {
            _logger.LogWarning("Server with id: {id} not found", id);
            throw new Exception("Server not found");
        }

        if (server.ServerStatus == ServerStatus.Rented)
        {
            server.ServerStatus = ServerStatus.Ready;
            server.RentedAt = null;
            server.StartedAt = null;

            await _serverDbContext.SaveChangesAsync();
            
            return new ServerResponseDto
            {
                Id = server.Id,
                OsName = server.OsName,
                RentedAt = server.RentedAt,
            };
        }

        throw new Exception("Server is not rented");
    }

    public async Task<string> GetStatus(Guid id)
    {
        var server = await _serverDbContext.Servers.FirstOrDefaultAsync(s => s.Id == id);

        if (server == null)
        {
            throw new Exception("Server not found");
        }

        var a = new Exception();

        return server.ServerStatus.ToString();
    }
}
