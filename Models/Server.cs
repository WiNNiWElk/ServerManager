namespace ServerManager.Models;

public class Server
{
    public Guid Id { get; set; }
    public string OsName { get; set; }
    public int RamGb { get; set; }
    public int StorageGb { get; set; }
    public int CpuCores { get; set; }
    public DateTime? RentedAt { get; set; }
    public ServerStatus ServerStatus { get; set; }
}