using Microsoft.AspNetCore.SignalR;

namespace ServerManager.DTOs;

public class ServerResponseDto
{
    public Guid Id { get; set; }
    public string OsName { get; set; }
    
    public DateTime? RentedAt { get; set;}
}