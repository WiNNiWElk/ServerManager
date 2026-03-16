namespace ServerManager.DTOs;

public class CreateServerDto
{
    public string OsName { get; set; }
    public int RamGb { get; set; }
    public int StorageGb { get; set; }
    public int CpuCores { get; set; }
}