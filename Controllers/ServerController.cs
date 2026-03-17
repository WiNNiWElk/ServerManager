using ServerManager.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Versioning;
using ServerManager.DTOs;
using ServerManager.Models;
using System.Security.Cryptography.X509Certificates;

namespace ServerManager.Controllers;

[ApiController]
[Route("servers")]
public class ServerController : ControllerBase
{
    private readonly ServerService _serverService;
    private readonly ILogger<ServerController> _logger;

    public ServerController(ServerService serverService, ILogger<ServerController> logger)
    {
        _serverService = serverService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<Server>> AddServer(CreateServerDto serverRequest)
    {
        var server = await _serverService.AddServer(serverRequest);
        return server;
    }

    [HttpGet("available")]
    public async Task<ActionResult<List<Server>>> Available(string osName, int? ramMemory, int? cpuCores)
    {
        var servers = await _serverService.FindAvailable(osName, ramMemory, cpuCores);
        return servers;
    }

    [HttpGet]
    public async Task<ActionResult<List<Server>>> ShowAll()
    {
        var servers = await _serverService.ShowAllServers();
        return servers;
    }

    [HttpDelete("{id}/delete")]
    public async Task<ActionResult> DeleteServer(Guid id)
    {
        await _serverService.DeleteServer(id);
        return NoContent();
    }

    [HttpPost("{id}/rent")]
    public async Task<ActionResult<Server>> Rent(Guid id)
    {
        var server = await _serverService.RentServer(id);
        return server;
    }

    [HttpPost("{id}/release")]
    public async Task<ActionResult<ServerResponseDto>> Release(Guid id)
    {
        var server = await _serverService.ReleaseServer(id);
        return server;
    }

    [HttpGet("{id}/status")]
    public async Task<string> GetStatus(Guid id)
    {
        var server = await _serverService.GetStatus(id);
        return server;
    }
}