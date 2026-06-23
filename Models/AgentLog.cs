namespace AgenticCrm.Models;

public class AgentLog
{
    public string Id { get; set; } = string.Empty;
    public string AgentId { get; set; } = string.Empty;
    public string LeadId { get; set; } = string.Empty;
    public string LeadName { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Type { get; set; } = "info";
    public string Message { get; set; } = string.Empty;
    public string? Payload { get; set; }
}
