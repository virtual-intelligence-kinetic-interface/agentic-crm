namespace AgenticCrm.Models;

public class Lead
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = "New";
    public string Source { get; set; } = string.Empty;
    public string Budget { get; set; } = string.Empty;
    public string CompanySize { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public string CoreProductInterest { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public DateTime LastActionDate { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }

    public int? SalesScore { get; set; }
    public string? SalesEmailDraft { get; set; }
    public string? SalesThought { get; set; }

    public string? MarketingPitch { get; set; }
    public string? MarketingCampaign { get; set; }
    public string? MarketingThought { get; set; }

    public List<string>? SuccessChecklist { get; set; }
    public string? SuccessResponse { get; set; }
    public string? SuccessThought { get; set; }
}
